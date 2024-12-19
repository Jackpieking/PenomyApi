using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;

public class Art20GetDetailToCreateChapResponse : IFeatureResponse
{
    public Artwork ArtworkDetail { get; set; }

    public Art20GetDetailToCreateChapResponseAppCode AppCode { get; set; }

    public static readonly Art20GetDetailToCreateChapResponse ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR = new()
    {
        AppCode = Art20GetDetailToCreateChapResponseAppCode.ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR
    };

    public static Art20GetDetailToCreateChapResponse SUCCESS(Artwork artworkDetail)
    {
        return new()
        {
            AppCode = Art20GetDetailToCreateChapResponseAppCode.SUCCESS,
            ArtworkDetail = artworkDetail
        };
    }
}
