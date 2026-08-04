using System.ComponentModel.DataAnnotations;

namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Request object used to create a new visit report.
/// </summary>
public class CreateVisitReportRequest
{
    /// <summary>
    /// Name of the visitor.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional position of the visitor.
    /// </summary>
    [MaxLength(255)]
    public string? Position { get; set; }

    /// <summary>
    /// Optional company of the visitor.
    /// </summary>
    [MaxLength(255)]
    public string? Company { get; set; }

    /// <summary>
    /// Optional email address of the visitor.
    /// </summary>
    [EmailAddress]
    [MaxLength(320)]
    public string? MailAddress { get; set; }

    /// <summary>
    /// Optional phone number of the visitor.
    /// </summary>
    [MaxLength(100)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Text content of the visit report.
    /// </summary>
    [Required]
    [MaxLength(5000)]
    public string ReportText { get; set; } = string.Empty;
}