namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.Common;

public class G63PaginationOptions
{
    /// <summary>
    ///     The default page size that will be used for pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 20;

    public bool AllowPagination { get; set; }

    public int TotalArtworks { get; set; }

    public int TotalPages { get; set; }
}
