namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.Common;

public class G48PaginationOptions
{
    /// <summary>
    ///     The default page size that will be used for pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 8;

    public bool AllowPagination { get; set; }

    public int TotalArtworks { get; set; }

    public int TotalPages { get; set; }
}