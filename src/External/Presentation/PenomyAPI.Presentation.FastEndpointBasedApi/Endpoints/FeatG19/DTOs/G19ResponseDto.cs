using PenomyAPI.App.FeatG19;
using PenomyAPI.App.FeatG9;
using ProtoBuf;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;

[ProtoContract]
public sealed class G19ResponseDto
{
    [ProtoMember(1)]
    public string AnimeId { get; set; }

    [ProtoMember(2)]
    public string ChapterId { get; set; }

    [ProtoMember(3)]
    public string Title { get; set; }

    [ProtoMember(5)]
    public string Description { get; set; }

    [ProtoMember(6)]
    public int UploadOrder { get; set; }

    [ProtoMember(7)]
    public bool AllowComment { get; set; }

    [ProtoMember(8)]
    public string CreatedBy { get; set; }

    [ProtoMember(9)]
    public long TotalViews { get; set; }

    public string VideoUrl { get; set; }

    public static G19ResponseDto MapFrom(G19Response response)
    {
        var chapterDetail = response.ChapterDetail;

        return new()
        {
            AnimeId = chapterDetail.ArtworkId.ToString(),
            ChapterId = chapterDetail.Id.ToString(),
            Title = chapterDetail.Title,
            Description = chapterDetail.Description,
            AllowComment = chapterDetail.AllowComment,
            CreatedBy = chapterDetail.CreatedBy.ToString(),
            UploadOrder = chapterDetail.UploadOrder,
            TotalViews = chapterDetail.TotalViews,
            VideoUrl = chapterDetail.ChapterVideoUrl
        };
    }
}
