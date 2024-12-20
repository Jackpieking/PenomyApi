using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG2;

public sealed class G2Response : IFeatureResponse
{
    public G2ResponseAppCode AppCode { get; set; }

    public ICollection<G2TopArtworkReadModel> TopArtworks { get; set; }

    public static G2Response SUCCESS(ICollection<G2TopArtworkReadModel> topRecommendedArtworks)
    {
        return new()
        {
            AppCode = G2ResponseAppCode.SUCCESS,
            TopArtworks = topRecommendedArtworks,
        };
    }

    public static readonly G2Response DATABASE_ERROR = new()
    {
        AppCode = G2ResponseAppCode.DATABASE_ERROR
    };
}
