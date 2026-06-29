using FairVisitReport.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FairVisitReport.Api.Data;

public class ApplicationDbContext : DbContext{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<VisitReportEntity> VisitReports => Set<VisitReportEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<VisitReportEntity>(entity =>{
            entity.ToTable("visit_reports");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Position)
                .HasColumnName("position")
                .HasMaxLength(255);

            entity.Property(x => x.Company)
                .HasColumnName("company")
                .HasMaxLength(255);

            entity.Property(x => x.MailAddress)
                .HasColumnName("mail_address")
                .HasMaxLength(320);

            entity.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(100);

            entity.Property(x => x.ReportText)
                .HasColumnName("report_text")
                .HasMaxLength(5000)
                .IsRequired();

            entity.Property(x => x.Exported)
                .HasColumnName("exported")
                .HasDefaultValue(false)
                .IsRequired();

            entity.Property(x => x.ExportedAt)
                .HasColumnName("exported_at");

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        });
    }
}