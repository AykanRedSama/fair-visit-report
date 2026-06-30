namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Request object used to export multiple visit reports by their identifiers.
/// </summary>
public class ExportRequest
{
    /// <summary>
    /// Technical identifiers of the visit reports that should be exported.
    /// </summary>
    public List<long> Ids { get; set; } = [];
}