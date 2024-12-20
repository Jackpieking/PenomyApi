using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt15;

public class Art15Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public long ArtworkId { get; set; }

    public Art15ResponseAppCode AppCode { get; set; }

    public static readonly Art15Response DATABASE_ERROR = new()
    {
        IsSuccess = false,
        AppCode = Art15ResponseAppCode.DATABASE_ERROR
    };

    public static readonly Art15Response FILE_SERVICE_ERROR = new()
    {
        IsSuccess = false,
        AppCode = Art15ResponseAppCode.FILE_SERVICE_ERROR
    };

    public static Art15Response SUCCESS(long artworkId) => new()
    {
        ArtworkId = artworkId,
        AppCode = Art15ResponseAppCode.SUCCESS,
    };
}
