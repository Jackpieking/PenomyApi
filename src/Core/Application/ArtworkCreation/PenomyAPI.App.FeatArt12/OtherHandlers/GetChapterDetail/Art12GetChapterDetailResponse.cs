using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;

public sealed class Art12GetChapterDetailResponse : IFeatureResponse
{
    public ArtworkChapter ChapterDetail { get; set; }

    public Art12GetChapterDetailResponseAppCode AppCode { get; set; }

    public static readonly Art12GetChapterDetailResponse CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = Art12GetChapterDetailResponseAppCode.CHAPTER_IS_NOT_FOUND,
    };

    public static readonly Art12GetChapterDetailResponse CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art12GetChapterDetailResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
    };

    public static readonly Art12GetChapterDetailResponse NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        AppCode = Art12GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
    };

    public static readonly Art12GetChapterDetailResponse DATABASE_ERROR = new()
    {
        AppCode = Art12GetChapterDetailResponseAppCode.DATABASE_ERROR,
    };

    public static Art12GetChapterDetailResponse SUCCESS(ArtworkChapter chapterDetail) => new()
    {
        ChapterDetail = chapterDetail,
        AppCode = Art12GetChapterDetailResponseAppCode.SUCCESS,
    };
}
