namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.Common;

public class SM7PaginationOptions
{
    /// <summary>
    ///     The default page size that will be used for pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 10;

    public bool AllowPagination { get; set; }

    public int TotalGroups { get; set; }

    public int TotalPages { get; set; }
}
