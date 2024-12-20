using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;

namespace PenomyAPI.App.FeatG19;

public class G19Response : IFeatureResponse
{
    public G19ResponseAppCode AppCode { get; set; }

    public G19AnimeChapterDetailReadModel ChapterDetail { get; set; }

    public static readonly G19Response CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = G19ResponseAppCode.CHAPTER_IS_NOT_FOUND,
    };

    public static G19Response SUCCESS(G19AnimeChapterDetailReadModel chapterDetail)
    {
        return new()
        {
            AppCode = G19ResponseAppCode.SUCCESS,
            ChapterDetail = chapterDetail
        };
    }
}
