using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG15;

namespace PenomyAPI.App.FeatG15;

public class G15Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G15AnimeDetailReadModel Result { get; set; }

    public G15ResponseAppCode AppCode { get; set; }

    public static G15Response SUCCESS(G15AnimeDetailReadModel animeDetail)
    {
        return new()
        {
            IsSuccess = true,
            AppCode = G15ResponseAppCode.SUCCESS,
            Result = animeDetail
        };
    }

    public static readonly G15Response ID_NOT_FOUND = new()
    {
        IsSuccess = false,
        AppCode = G15ResponseAppCode.NOT_FOUND,
    };
}
