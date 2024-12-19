using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;

public class G19GetChapterListResponse : IFeatureResponse
{
    public G19GetChapterListResponseAppCode AppCode { get; set; }

    public List<G19AnimeChapterItemReadModel> ChapterList { get; set; }

    public static G19GetChapterListResponse SUCCESS(List<G19AnimeChapterItemReadModel> chapters)
    {
        return new G19GetChapterListResponse()
        {
            AppCode = G19GetChapterListResponseAppCode.SUCCESS,
            ChapterList = chapters,
        };
    }

    public static readonly G19GetChapterListResponse ANIME_ID_NOT_FOUND =
        new G19GetChapterListResponse()
        {
            AppCode = G19GetChapterListResponseAppCode.ANIME_ID_NOT_FOUND,
        };
}
