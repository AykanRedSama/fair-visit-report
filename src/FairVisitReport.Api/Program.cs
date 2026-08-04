using FairVisitReport.Api.Data;
using FairVisitReport.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyslogLogging;

var builder = WebApplication.CreateBuilder(args);

var syslogHost = builder.Configuration["Syslog:Host"];

var syslogPort = int.TryParse(
    builder.Configuration["Syslog:Port"],
    out var configuredSyslogPort)
    ? configuredSyslogPort
    : 514;

if (!string.IsNullOrWhiteSpace(syslogHost))
{
    builder.Logging.AddSyslog(
        syslogHost,
        syslogPort,
        false);
}

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("Validation");

            var errorCount = context.ModelState.Values
                .SelectMany(value => value.Errors)
                .Count();

            logger.LogWarning(
                "Request validation failed with {ValidationErrorCount} errors and correlation id {CorrelationId}",
                errorCount,
                context.HttpContext.TraceIdentifier);

            return new BadRequestObjectResult(new
            {
                error = "Request validation failed",
                statusCode = 400,
                timestamp = DateTimeOffset.UtcNow,
                correlationId = context.HttpContext.TraceIdentifier
            });
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<VisitReportService>();
builder.Services.AddScoped<ExportService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.Logger.LogInformation(
    "Application started with correlation id {CorrelationId}",
    "system");

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation(
        "Application stopping with correlation id {CorrelationId}",
        "system");
});

app.Use(async (context, next) =>
{
    var correlationId =
        context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

    if (string.IsNullOrWhiteSpace(correlationId) ||
        correlationId.Length > 100)
    {
        correlationId = Guid.NewGuid().ToString("N");
    }

    context.TraceIdentifier = correlationId;
    context.Response.Headers["X-Correlation-ID"] = correlationId;

    await next();

    if (context.Request.Path != "/health")
    {
        app.Logger.LogInformation(
            "HTTP request completed with method {RequestMethod}, path {RequestPath}, status code {StatusCode} and correlation id {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            correlationId);
    }
});

app.UseExceptionHandler(errorApplication =>
{
    errorApplication.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()
            ?.Error;

        app.Logger.LogError(
            exception,
            "Unexpected error while processing the request with correlation id {CorrelationId}",
            context.TraceIdentifier);

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected server error occurred",
            statusCode = 500,
            timestamp = DateTimeOffset.UtcNow,
            correlationId = context.TraceIdentifier
        });
    });
});

app.UseCors("frontend");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();