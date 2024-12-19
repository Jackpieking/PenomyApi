using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt16;

public class Art16Response : IFeatureResponse
{
    public Artwork AnimeDetail { get; set; }

    public Art16ResponseAppCode AppCode { get; set; }

    public static Art16Response SUCCESS(Artwork artworkDetail)
    {
        return new()
        {
            AppCode = Art16ResponseAppCode.SUCCESS,
            AnimeDetail = artworkDetail,
        };
    }

    public static readonly Art16Response ARTWORK_ID_NOT_FOUND = new()
    {
        AppCode = Art16ResponseAppCode.ARTWORK_ID_NOT_FOUND,
    };

    public static readonly Art16Response ARTWORK_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art16ResponseAppCode.ARTWORK_IS_TEMPORARILY_REMOVED
    };

    public static readonly Art16Response ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR = new()
    {
        AppCode = Art16ResponseAppCode.ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR
    };
}
