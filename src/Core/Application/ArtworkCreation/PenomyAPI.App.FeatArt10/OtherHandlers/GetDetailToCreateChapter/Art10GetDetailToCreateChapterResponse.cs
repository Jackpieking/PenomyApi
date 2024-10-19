using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;

public sealed class Art10GetDetailToCreateChapterResponse : IFeatureResponse
{
    public Artwork ComicDetail { get; set; }

    public Art10GetDetailToCreateChapterResponseAppCode AppCode { get; set; }

    public static readonly Art10GetDetailToCreateChapterResponse COMIC_ID_NOT_FOUND;

    static Art10GetDetailToCreateChapterResponse()
    {
        COMIC_ID_NOT_FOUND = new()
        {
            AppCode = Art10GetDetailToCreateChapterResponseAppCode.COMIC_ID_NOT_FOUND,
        };
    }
}
