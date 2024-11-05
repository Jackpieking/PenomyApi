using PenomyAPI.App.FeatG9;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

public sealed class G9ResponseDto
{
    public string ComicId { get; set; }

    public string ChapterId { get; set; }

    public string Title { get; set; }

    public string ComicTitle { get; set; }

    public string Description { get; set; }

    public int UploadOrder { get; set; }

    public bool AllowComment { get; set; }

    public string CreatedBy { get; set; }

    public long TotalFavorites { get; set; }

    public IEnumerable<ComicChapterImageItemDto> Images { get; set; }

    public static G9ResponseDto MapFrom(G9Response response)
    {
        var chapterDetail = response.ComicChapterDetail;

        return new()
        {
            ComicId = chapterDetail.ArtworkId.ToString(),
            ChapterId = chapterDetail.Id.ToString(),
            ComicTitle = chapterDetail.BelongedArtwork.Title,
            Title = chapterDetail.Title,
            Description = chapterDetail.Description,
            AllowComment = chapterDetail.AllowComment,
            CreatedBy = chapterDetail.CreatedBy.ToString(),
            UploadOrder = chapterDetail.UploadOrder,
            TotalFavorites = chapterDetail.ChapterMetaData.TotalFavorites,
            Images = chapterDetail.ChapterMedias.Select(ComicChapterImageItemDto.MapFrom)
        };
    }
}
