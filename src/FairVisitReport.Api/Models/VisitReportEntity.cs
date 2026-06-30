using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FairVisitReport.Api.Models;

/// <summary>
/// Represents a visit report stored in the database.
/// </summary>
[Table("visit_reports")]
public class VisitReport
{
    /// <summary>
    /// Technical primary key of the visit report.
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// Name of the visitor.
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional position of the visitor.
    /// </summary>
    [MaxLength(255)]
    [Column("position")]
    public string? Position { get; set; }

    /// <summary>
    /// Optional company of the visitor.
    /// </summary>
    [MaxLength(255)]
    [Column("company")]
    public string? Company { get; set; }

    /// <summary>
    /// Optional email address of the visitor.
    /// </summary>
    [MaxLength(320)]
    [Column("mail_address")]
    public string? MailAddress { get; set; }

    /// <summary>
    /// Optional phone number of the visitor.
    /// </summary>
    [MaxLength(100)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Text content of the visit report.
    /// </summary>
    [Required]
    [MaxLength(5000)]
    [Column("report_text")]
    public string ReportText { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the report has already been exported.
    /// </summary>
    [Required]
    [Column("exported")]
    public bool Exported { get; set; } = false;

    /// <summary>
    /// Timestamp when the report was exported.
    /// </summary>
    [Column("exported_at")]
    public DateTimeOffset? ExportedAt { get; set; }

    /// <summary>
    /// Timestamp when the report was created.
    /// </summary>
    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the report was last updated.
    /// </summary>
    [Required]
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}