using FairVisitReport.Api.Data;
using FairVisitReport.Api.Services;
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

builder.Services.AddControllers();
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

app.Logger.LogInformation("Application started");

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation("Application stopping");
});

app.UseCors("frontend");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();