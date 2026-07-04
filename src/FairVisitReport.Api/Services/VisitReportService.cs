using FairVisitReport.Api.Data;
using FairVisitReport.Api.DTOs;
using FairVisitReport.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FairVisitReport.Api.Services;

/// <summary>
/// Provides business logic for creating, reading, updating and deleting visit reports.
/// </summary>
public class VisitReportService
{
    private readonly ApplicationDbContext db;

    /// <summary>
    /// Creates a new visit report service.
    /// </summary>
    public VisitReportService(ApplicationDbContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Creates a new visit report and stores it in the database.
    /// </summary>
    public async Task<VisitReportDto> CreateAsync(CreateVisitReportRequest request)
    {
        var now = DateTimeOffset.UtcNow;

        var entity = new VisitReport
        {
            Name = request.Name.Trim(),
            Position = request.Position?.Trim(),
            Company = request.Company?.Trim(),
            MailAddress = request.MailAddress?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            ReportText = request.ReportText.Trim(),
            Exported = false,
            ExportedAt = null,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.VisitReports.Add(entity);
        await db.SaveChangesAsync();

        return ToDto(entity);
    }

    /// <summary>
    /// Returns a paginated, filterable and sortable list of visit reports.
    /// </summary>
    public async Task<PaginatedResult<VisitReportDto>> GetAllAsync(
        bool? exported,
        string? company,
        string? name,
        int page,
        int pageSize,
        string? sortBy,
        string? sortDirection)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 25 : pageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var query = db.VisitReports.AsQueryable();

        if (exported.HasValue)
        {
            query = query.Where(x => x.Exported == exported.Value);
        }

        if (!string.IsNullOrWhiteSpace(company))
        {
            var companyFilter = company.Trim().ToLower();
            query = query.Where(x => x.Company != null && x.Company.ToLower().Contains(companyFilter));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var nameFilter = name.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(nameFilter));
        }

        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLower() switch
        {
            "name" => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "company" => descending ? query.OrderByDescending(x => x.Company) : query.OrderBy(x => x.Company),
            "createdat" => descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => ToDto(x))
            .ToListAsync();

        return new PaginatedResult<VisitReportDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    /// <summary>
    /// Returns a single visit report by its technical identifier.
    /// </summary>
    public async Task<VisitReportDto?> GetByIdAsync(long id)
    {
        var entity = await db.VisitReports.FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        return ToDto(entity);
    }

    /// <summary>
    /// Updates an existing visit report.
    /// </summary>
    public async Task<VisitReportDto?> UpdateAsync(long id, UpdateVisitReportRequest request)
    {
        var entity = await db.VisitReports.FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        entity.Name = request.Name.Trim();
        entity.Position = request.Position?.Trim();
        entity.Company = request.Company?.Trim();
        entity.MailAddress = request.MailAddress?.Trim();
        entity.PhoneNumber = request.PhoneNumber?.Trim();
        entity.ReportText = request.ReportText.Trim();
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();

        return ToDto(entity);
    }

    /// <summary>
    /// Deletes a single visit report if it has already been exported.
    /// </summary>
    public async Task<string> DeleteAsync(long id)
    {
        var entity = await db.VisitReports.FindAsync(id);

        if (entity == null)
        {
            return "not_found";
        }

        if (!entity.Exported)
        {
            return "not_exported";
        }

        db.VisitReports.Remove(entity);
        await db.SaveChangesAsync();

        return "deleted";
    }

    /// <summary>
    /// Deletes all visit reports that have already been exported.
    /// </summary>
    public async Task<int> DeleteAllExportedAsync()
    {
        var entities = await db.VisitReports
            .Where(x => x.Exported)
            .ToListAsync();

        db.VisitReports.RemoveRange(entities);
        await db.SaveChangesAsync();

        return entities.Count;
    }

    private static VisitReportDto ToDto(VisitReport entity)
    {
        return new VisitReportDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Position = entity.Position,
            Company = entity.Company,
            MailAddress = entity.MailAddress,
            PhoneNumber = entity.PhoneNumber,
            ReportText = entity.ReportText,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Exported = entity.Exported,
            ExportedAt = entity.ExportedAt
        };
    }
}