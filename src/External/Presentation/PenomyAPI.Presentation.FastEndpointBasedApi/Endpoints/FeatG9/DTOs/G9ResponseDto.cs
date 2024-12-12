using System.Collections.Generic;
using System.Linq;
using PenomyAPI.App.FeatG9;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

[ProtoContract]
public sealed class G9ResponseDto
{
    [ProtoMember(1)]
    public string ComicId { get; set; }

    [ProtoMember(2)]
    public string ChapterId { get; set; }

    [ProtoMember(3)]
    public string Title { get; set; }

    [ProtoMember(4)]
    public string ComicTitle { get; set; }

    [ProtoMember(5)]
    public string Description { get; set; }

    [ProtoMember(6)]
    public int UploadOrder { get; set; }

    [ProtoMember(7)]
    public bool AllowComment { get; set; }

    [ProtoMember(8)]
    public string CreatedBy { get; set; }

    [ProtoMember(9)]
    public long TotalFavorites { get; set; }

    [ProtoMember(10)]
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
