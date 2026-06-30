namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Response object returned by the API for a visit report.
/// </summary>
public class VisitReportDto
{
    /// <summary>
    /// Technical identifier of the visit report.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the visitor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional position of the visitor.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Optional company of the visitor.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Optional email address of the visitor.
    /// </summary>
    public string? MailAddress { get; set; }

    /// <summary>
    /// Optional phone number of the visitor.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Text content of the visit report.
    /// </summary>
    public string ReportText { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the visit report was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the visit report was last updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the visit report has already been exported.
    /// </summary>
    public bool Exported { get; set; }

    /// <summary>
    /// Timestamp when the visit report was exported.
    /// </summary>
    public DateTimeOffset? ExportedAt { get; set; }
}