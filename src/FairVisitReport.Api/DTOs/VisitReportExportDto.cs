namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Response object returned when one or more visit reports are exported.
/// </summary>
public class VisitReportExportResponseDto
{
    /// <summary>
    /// Timestamp when the export operation was executed.
    /// </summary>
    public DateTimeOffset ExportedAt { get; set; }

    /// <summary>
    /// Visit reports included in the export response.
    /// </summary>
    public List<VisitReportExportDto> Reports { get; set; } = [];
}

/// <summary>
/// Represents a single visit report inside an export response.
/// </summary>
public class VisitReportExportDto
{
    /// <summary>
    /// Technical identifier of the exported visit report.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Visitor information related to the exported visit report.
    /// </summary>
    public VisitorExportDto Visitor { get; set; } = new();

    /// <summary>
    /// Text content of the exported visit report.
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
    /// Timestamp when the visit report was exported.
    /// </summary>
    public DateTimeOffset? ExportedAt { get; set; }
}

/// <summary>
/// Visitor information used inside the export response.
/// </summary>
public class VisitorExportDto
{
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
}