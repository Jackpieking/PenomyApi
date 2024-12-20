using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;

public sealed class Art12ReloadChapterMediaResponse : IFeatureResponse
{
    public Art12ReloadChapterMediaResponseAppCode AppCode { get; set; }

    public List<ArtworkChapterMedia> ChapterMedias { get; set; }

    public static readonly Art12ReloadChapterMediaResponse CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = Art12ReloadChapterMediaResponseAppCode.CHAPTER_IS_NOT_FOUND
    };

    public static readonly Art12ReloadChapterMediaResponse DATABASE_ERROR = new()
    {
        AppCode = Art12ReloadChapterMediaResponseAppCode.DATABASE_ERROR
    };

    public static Art12ReloadChapterMediaResponse SUCCESS(List<ArtworkChapterMedia> chapterMedias) => new()
    {
        AppCode = Art12ReloadChapterMediaResponseAppCode.SUCCESS,
        ChapterMedias = chapterMedias
    };
}
