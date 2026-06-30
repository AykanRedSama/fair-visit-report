using System.ComponentModel.DataAnnotations;

namespace FairVisitReport.Api.DTOs;

public class UpdateVisitReportRequest
{
   
    public string Name { get; set; } = string.Empty;

   
    public string? Position { get; set; }

    
    public string? Company { get; set; }

    
    
    public string? MailAddress { get; set; }

    
    public string? PhoneNumber { get; set; }

    
    public string ReportText { get; set; } = string.Empty;
}