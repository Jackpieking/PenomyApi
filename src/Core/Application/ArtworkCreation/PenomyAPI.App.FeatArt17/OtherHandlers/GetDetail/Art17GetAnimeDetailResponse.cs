using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;

public class Art17GetAnimeDetailResponse : IFeatureResponse
{
    public Artwork AnimeDetail { get; set; }

    public Art17GetDetailResponseAppCode AppCode { get; set; }

    public static readonly Art17GetAnimeDetailResponse ID_NOT_FOUND = new()
    {
        AppCode = Art17GetDetailResponseAppCode.ID_NOT_FOUND,
    };

    public static readonly Art17GetAnimeDetailResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = Art17GetDetailResponseAppCode.CREATOR_HAS_NO_PERMISSION,
    };
}
