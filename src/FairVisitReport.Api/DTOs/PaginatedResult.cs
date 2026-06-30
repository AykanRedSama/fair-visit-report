namespace FairVisitReport.Api.DTOs;

/// <summary>
/// Represents a paginated API result.
/// </summary>
/// <typeparam name="T">The type of items contained in the result.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Items returned for the current page.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items matching the query.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of available pages.
    /// </summary>
    public int TotalPages { get; set; }
}