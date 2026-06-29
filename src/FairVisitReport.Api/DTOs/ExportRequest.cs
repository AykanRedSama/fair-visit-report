using System.ComponentModel.DataAnnotations;

namespace FairVisitReport.Api.DTOs;

public class ExportRequest{
    [Required]
    public List<long> Ids {get; set;} = [];
}