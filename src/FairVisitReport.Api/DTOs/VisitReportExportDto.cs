namespace FairVisitReport.Api.DTOs;

public class VisitReportExportResponseDto
{
    public DateTimeOffset ExportedAt { get; set; }
    public List<VisitReportExportDto> Reports { get; set; } = [];
}

public class VisitReportExportDto
{
    public long Id { get; set; }
    public VisitorExportDto Visitor { get; set; } = new();
    public string ReportText { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? ExportedAt { get; set; }
}

public class VisitorExportDto
{
    public string Name { get; set; } = string.Empty;
    public string? Position { get; set; }
    public string? Company { get; set; }
    public string? MailAddress { get; set; }
    public string? PhoneNumber { get; set; }
}