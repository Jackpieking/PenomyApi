using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG9;

public sealed class G9Response : IFeatureResponse
{
    public G9ResponseAppCode AppCode { get; set; }

    public ArtworkChapter ComicChapterDetail { get; set; }

    public static readonly G9Response CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = G9ResponseAppCode.CHAPTER_IS_NOT_FOUND,
    };

    public static G9Response SUCCESS(ArtworkChapter chapterDetail)
    {
        return new()
        {
            AppCode = G9ResponseAppCode.SUCCESS,
            ComicChapterDetail = chapterDetail
        };
    }
}
