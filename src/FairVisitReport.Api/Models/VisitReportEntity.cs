namespace FairVisitReport.Api.Models;

public class VisitReportEntity{
    public long Id {get; set;}

    public string Name {get;set;} = string.Empty;
    public string? Position {get; set;}
    public string? Company {get; set;}
    public string? MailAddress {get; set;}
    public string? PhoneNumber {get; set;}
    public string ReportText {get; set;} = string.Empty;
    public bool Exported {get; set;}
    public DateTimeOffset? ExportedAt {get; set;}
    public DateTimeOffset CreatedAt {get;set;}
    public DateTimeOffset UpdatedAt {get;set;}
}