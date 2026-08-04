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
}