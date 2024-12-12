using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG9;

namespace PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;

public class G9GetChapterListResponse : IFeatureResponse
{
    public G9GetChapterListResponseAppCode AppCode { get; set; }

    public List<G9ChapterItemReadModel> ChapterList { get; set; }

    public static G9GetChapterListResponse SUCCESS(List<G9ChapterItemReadModel> chapters)
    {
        return new G9GetChapterListResponse()
        {
            AppCode = G9GetChapterListResponseAppCode.SUCCESS,
            ChapterList = chapters,
        };
    }

    public static readonly G9GetChapterListResponse COMIC_ID_NOT_FOUND =
        new G9GetChapterListResponse()
        {
            AppCode = G9GetChapterListResponseAppCode.COMIC_ID_NOT_FOUND,
        };
}
