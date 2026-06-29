using FairVisitReport.Api.DTOs;
using FairVisitReport.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FairVisitReport.Api.Controllers;

[ApiController]
[Route("api/visit-reports")]
public class VisitReportsController : ControllerBase
{
    private readonly VisitReportService service;
    private readonly ExportService exportService;

    public VisitReportsController(VisitReportService service, ExportService exportService)
    {
        this.service = service;
        this.exportService = exportService;
    }

    [HttpPost]
    public async Task<ActionResult<VisitReportDto>> Create(CreateVisitReportRequest request)
    {
        var result = await service.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<VisitReportDto>>> GetAll(
        [FromQuery] bool? exported,
        [FromQuery] string? company,
        [FromQuery] string? name,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = "createdAt",
        [FromQuery] string? sortDirection = "desc")
    {
        var result = await service.GetAllAsync(
            exported,
            company,
            name,
            page,
            pageSize,
            sortBy,
            sortDirection);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VisitReportDto>> GetById(long id)
    {
        var result = await service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                error = "Visit report not found",
                statusCode = 404,
                timestamp = DateTimeOffset.UtcNow 
            });
        }

        return Ok(result);
    }
[HttpPut("{id:long}")]
    public async Task<ActionResult<VisitReportDto>> Update(long id, UpdateVisitReportRequest request)
    {
        var result = await service.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(new
            {
                error = "Visit report not found",
                statusCode = 404,
                timestamp = DateTimeOffset.UtcNow
            });
        }

        return Ok(result);
    }

    [HttpGet("{id:long}/export")]
    public async Task<ActionResult<VisitReportExportResponseDto>> ExportSingle(long id)
    {
        var result = await exportService.ExportSingleAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                error = "Visit report not found",
                statusCode = 404,
                timestamp = DateTimeOffset.UtcNow
            });
        }

        return Ok(result);
    }
    [HttpPost("export")]
    public async Task<ActionResult<VisitReportExportResponseDto>> ExportMany(ExportRequest request)
    {
        if (request.Ids.Count == 0)
        {
            return BadRequest(new
            {
                error = "At least one visit report id is required",
                statusCode = 400,
                timestamp = DateTimeOffset.UtcNow
            });
        }

        var result = await exportService.ExportManyAsync(request.Ids);

        if (result == null)
        {
            return NotFound(new
            {
                error = "One or more visit reports were not found",
                statusCode = 404,
                timestamp = DateTimeOffset.UtcNow
            });
        }

        return Ok(result);
    }

    [HttpPost("export-unexported")]
    public async Task<ActionResult<VisitReportExportResponseDto>> ExportUnexported()
    {
        var result = await exportService.ExportUnexportedAsync();

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await service.DeleteAsync(id);

        if (result == "not_found")
        {
            return NotFound(new
            {
                error = "Visit report not found",
                statusCode = 404,
                timestamp = DateTimeOffset.UtcNow
            });
        }
        if (result == "not_exported")
        {
            return Conflict(new
            {
                error = "Visit report has not been exported",
                statusCode = 409,
                timestamp = DateTimeOffset.UtcNow
            });
        }

        return NoContent();
    }

    [HttpDelete("exported")]
    public async Task<IActionResult> DeleteAllExported()
    {
        var deletedCount = await service.DeleteAllExportedAsync();

        return Ok(new
        {
            deletedCount
        });
    }
}
