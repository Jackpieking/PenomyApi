namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;

public class Art1PaginationOptions
{
    /// <summary>
    ///     The default page size that will be used for pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 4;

    public bool AllowPagination { get; set; }

    public int TotalArtworks { get; set; }

    public int TotalPages { get; set; }
}
