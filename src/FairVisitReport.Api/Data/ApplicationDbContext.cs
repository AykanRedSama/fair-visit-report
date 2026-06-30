
using FairVisitReport.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FairVisitReport.Api.Data;

/// <summary>
/// Database context used by Entity Framework Core.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Creates a new database context using configured options.
    /// </summary>
    /// <param name="options">Database context options configured in Program.cs.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Database set for visit reports.
    /// </summary>
    public DbSet<VisitReportEntity> VisitReports => Set<VisitReportEntity>();
}
