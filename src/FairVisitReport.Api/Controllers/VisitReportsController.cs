using FairVisitReport.Api.DTOs;
using FairVisitReport.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FairVisitReport.Api.Controllers;

/// <summary>
/// Provides HTTP endpoints for managing visit reports.
/// </summary>
[ApiController]
[Route("api/visit-reports")]
public class VisitReportsController : ControllerBase
{
    private readonly VisitReportService service;
    private readonly ExportService exportService;

    /// <summary>
    /// Creates a new visit reports controller.
    /// </summary>
    /// <param name="service">Service used for visit report business logic.</param>
    /// <param name="exportService">Service used for visit report export logic.</param>
    public VisitReportsController(VisitReportService service, ExportService exportService)
    {
        this.service = service;
        this.exportService = exportService;
    }

    /// <summary>
    /// Creates a new visit report.
    /// </summary>
    /// <param name="request">The data required to create a visit report.</param>
    /// <returns>The created visit report.</returns>
    [HttpPost]
    public async Task<ActionResult<VisitReportDto>> Create(CreateVisitReportRequest request)
    {
        var result = await service.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Returns a paginated, filterable and sortable list of visit reports.
    /// </summary>
    /// <param name="exported">Optional export status filter.</param>
    /// <param name="company">Optional company filter.</param>
    /// <param name="name">Optional visitor name filter.</param>
    /// <param name="page">Requested page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="sortBy">Field used for sorting.</param>
    /// <param name="sortDirection">Sorting direction.</param>
    /// <returns>A paginated result containing visit reports.</returns>
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

    /// <summary>
    /// Returns a single visit report by its technical identifier.
    /// </summary>
    /// <param name="id">Technical identifier of the visit report.</param>
    /// <returns>The requested visit report.</returns>
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

    /// <summary>
    /// Updates an existing visit report.
    /// </summary>
    /// <param name="id">Technical identifier of the visit report.</param>
    /// <param name="request">Updated visit report data.</param>
    /// <returns>The updated visit report.</returns>
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

    /// <summary>
    /// Exports a single visit report by its technical identifier.
    /// </summary>
    /// <param name="id">Technical identifier of the visit report.</param>
    /// <returns>The export response for the requested visit report.</returns>
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

    /// <summary>
    /// Exports multiple visit reports by their technical identifiers.
    /// </summary>
    /// <param name="request">Request containing the identifiers of the visit reports to export.</param>
    /// <returns>The export response containing the exported visit reports.</returns>
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

    /// <summary>
    /// Exports all visit reports that have not been exported yet.
    /// </summary>
    /// <returns>The export response containing all previously unexported visit reports.</returns>
    [HttpPost("export-unexported")]
    public async Task<ActionResult<VisitReportExportResponseDto>> ExportUnexported()
    {
        var result = await exportService.ExportUnexportedAsync();

        return Ok(result);
    }

    /// <summary>
    /// Deletes a single visit report if it has already been exported.
    /// </summary>
    /// <param name="id">Technical identifier of the visit report.</param>
    /// <returns>No content if the visit report was deleted.</returns>
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

    /// <summary>
    /// Deletes all visit reports that have already been exported.
    /// </summary>
    /// <returns>The number of deleted visit reports.</returns>
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