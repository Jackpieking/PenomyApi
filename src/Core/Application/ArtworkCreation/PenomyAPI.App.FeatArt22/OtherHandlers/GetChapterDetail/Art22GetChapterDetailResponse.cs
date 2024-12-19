using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;

public class Art22GetChapterDetailResponse : IFeatureResponse
{
    public ArtworkChapter ChapterDetail { get; set; }

    public Art22GetChapterDetailResponseAppCode AppCode { get; set; }

    public static readonly Art22GetChapterDetailResponse CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art22GetChapterDetailResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
    };

    public static readonly Art22GetChapterDetailResponse NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        AppCode = Art22GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
    };

    public static readonly Art22GetChapterDetailResponse DATABASE_ERROR = new()
    {
        AppCode = Art22GetChapterDetailResponseAppCode.DATABASE_ERROR,
    };

    public static Art22GetChapterDetailResponse SUCCESS(ArtworkChapter chapterDetail) => new()
    {
        ChapterDetail = chapterDetail,
        AppCode = Art22GetChapterDetailResponseAppCode.SUCCESS,
    };
}
