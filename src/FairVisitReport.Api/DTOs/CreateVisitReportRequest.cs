using System.ComponentModel.DataAnnotations;

namespace FairVisitReport.Api.DTOs;

public class CreateVisitReportRequest{
    [Required]
    [MaxLength(255)]
    public string Name { get;set;} = string.Empty;

    [MaxLength(255)]
    public string? Position {get; set;}

    [MaxLength(255)]
    public string? Company {get; set;}
    
    [EmailAddress]
    [MaxLength(320)]
    public string? MailAddress {get; set;}

    [MaxLength(100)]
    public string? PhoneNumber {get; set;}

    [Required]
    [MaxLength(5000)]
    public string ReportText {get; set;} = string.Empty;
    
}