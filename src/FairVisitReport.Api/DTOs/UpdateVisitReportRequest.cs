namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Request object used to update an existing visit report.
/// </summary>
public class UpdateVisitReportRequest
{
    /// <summary>
    /// Updated name of the visitor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Updated optional position of the visitor.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Updated optional company of the visitor.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Updated optional email address of the visitor.
    /// </summary>
    public string? MailAddress { get; set; }

    /// <summary>
    /// Updated optional phone number of the visitor.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Updated text content of the visit report.
    /// </summary>
    public string ReportText { get; set; } = string.Empty;
}