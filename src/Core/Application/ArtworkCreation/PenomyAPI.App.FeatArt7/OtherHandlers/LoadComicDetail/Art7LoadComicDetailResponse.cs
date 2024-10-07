using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailResponse : IFeatureResponse
{
    public Artwork ComicDetail { get; set; }

    public Art7LoadComicDetailResponseStatusCode StatusCode { get; set; }

    public static readonly Art7LoadComicDetailResponse ComicIdNotFound = new()
    {
        StatusCode = Art7LoadComicDetailResponseStatusCode.ID_NOT_FOUND,
    };
}
