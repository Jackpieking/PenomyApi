using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;

namespace PenomyAPI.App.FeatG2;

public sealed class G2Response : IFeatureResponse
{
    public G2ResponseAppCode AppCode { get; set; }

    public G2TopRecommendedArtworks TopRecommendedArtworks { get; set; }

    public static G2Response SUCCESS(G2TopRecommendedArtworks topRecommendedArtworks)
    {
        return new()
        {
            AppCode = G2ResponseAppCode.SUCCESS,
            TopRecommendedArtworks = topRecommendedArtworks,
        };
    }

    public static readonly G2Response DATABASE_ERROR = new()
    {
        AppCode = G2ResponseAppCode.DATABASE_ERROR
    };
}
