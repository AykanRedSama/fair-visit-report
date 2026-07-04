using FairVisitReport.Api.Data;
using FairVisitReport.Api.DTOs;
using FairVisitReport.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FairVisitReport.Api.Services;

/// <summary>
/// Provides business logic for exporting visit reports.
/// </summary>
public class ExportService
{
    private readonly ApplicationDbContext db;

    /// <summary>
    /// Creates a new export service.
    /// </summary>
    public ExportService(ApplicationDbContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Exports a single visit report by its technical identifier.
    /// </summary>
    public async Task<VisitReportExportResponseDto?> ExportSingleAsync(long id)
    {
        var entity = await db.VisitReports.FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        var exportedAt = DateTimeOffset.UtcNow;

        entity.Exported = true;
        entity.ExportedAt = exportedAt;
        entity.UpdatedAt = exportedAt;

        await db.SaveChangesAsync();

        return CreateResponse([entity], exportedAt);
    }

    /// <summary>
    /// Exports multiple visit reports by their technical identifiers.
    /// </summary>
    public async Task<VisitReportExportResponseDto?> ExportManyAsync(List<long> ids)
    {
        var distinctIds = ids.Distinct().ToList();

        var entities = await db.VisitReports
            .Where(x => distinctIds.Contains(x.Id))
            .ToListAsync();

        if (entities.Count != distinctIds.Count)
        {
            return null;
        }

        var exportedAt = DateTimeOffset.UtcNow;

        foreach (var entity in entities)
        {
            entity.Exported = true;
            entity.ExportedAt = exportedAt;
            entity.UpdatedAt = exportedAt;
        }

        await db.SaveChangesAsync();

        return CreateResponse(entities, exportedAt);
    }

    /// <summary>
    /// Exports all visit reports that have not been exported yet.
    /// </summary>
    public async Task<VisitReportExportResponseDto> ExportUnexportedAsync()
    {
        var entities = await db.VisitReports
            .Where(x => !x.Exported)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        var exportedAt = DateTimeOffset.UtcNow;

        foreach (var entity in entities)
        {
            entity.Exported = true;
            entity.ExportedAt = exportedAt;
            entity.UpdatedAt = exportedAt;
        }

        await db.SaveChangesAsync();

        return CreateResponse(entities, exportedAt);
    }

    private static VisitReportExportResponseDto CreateResponse(List<VisitReport> entities, DateTimeOffset exportedAt)
    {
        return new VisitReportExportResponseDto
        {
            ExportedAt = exportedAt,
            Reports = entities.Select(ToExportDto).ToList()
        };
    }

    private static VisitReportExportDto ToExportDto(VisitReport entity)
    {
        return new VisitReportExportDto
        {
            Id = entity.Id,
            Visitor = new VisitorExportDto
            {
                Name = entity.Name,
                Position = entity.Position,
                Company = entity.Company,
                MailAddress = entity.MailAddress,
                PhoneNumber = entity.PhoneNumber
            },
            ReportText = entity.ReportText,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            ExportedAt = entity.ExportedAt
        };
    }
}