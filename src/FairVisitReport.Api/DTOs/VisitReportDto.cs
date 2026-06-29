namespace FairVisitReport.Api.DTOs;

public class VisitReportDto{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Position { get; set; }
    public string? Company { get; set; }
    public string? MailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string ReportText { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool Exported { get; set; }
    public DateTimeOffset? ExportedAt { get; set; }
}