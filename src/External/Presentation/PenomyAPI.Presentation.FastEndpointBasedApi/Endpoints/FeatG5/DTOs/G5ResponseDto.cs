using System.Collections.Generic;
using System.Linq;
using PenomyAPI.App.FeatG5;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;

[ProtoContract]
public class G5ResponseDto
{
    [ProtoMember(1)]
    public string Id { get; set; }

    [ProtoMember(2)]
    public string Title { get; set; }

    [ProtoMember(3)]
    public bool HasSeries { get; set; }

    [ProtoMember(4)]
    public string CreatorId { get; set; }

    [ProtoMember(5)]
    public string CountryId { get; set; }

    [ProtoMember(6)]
    public string CountryName { get; set; }

    [ProtoMember(7)]
    public IEnumerable<CategoryDto> Categories { get; set; }

    [ProtoMember(8)]
    public ArtworkStatus ArtworkStatus { get; set; }

    [ProtoMember(9)]
    public string SeriesId { get; set; }

    [ProtoMember(10)]
    public string SeriesName { get; set; }

    [ProtoMember(11)]
    public string ThumbnailUrl { get; set; }

    [ProtoMember(12)]
    public string Introduction { get; set; }

    [ProtoMember(13)]
    public bool IsAllowComment { get; set; }

    public static G5ResponseDto MapFrom(G5Response featResponse)
    {
        var comicDetail = featResponse.Result;

        var responseDto = new G5ResponseDto
        {
            Id = comicDetail.Id.ToString(),
            Title = comicDetail.Title,
            ThumbnailUrl = comicDetail.ThumbnailUrl,
            Introduction = comicDetail.Introduction,
            CountryId = comicDetail.CountryId.ToString(),
            CountryName = comicDetail.CountryName,
            HasSeries = comicDetail.HasSeries,
            ArtworkStatus = comicDetail.ArtworkStatus,
            IsAllowComment = comicDetail.AllowComment,
            // Creator detail section.
            CreatorId = comicDetail.CreatorId.ToString(),
        };

        //if (comicDetail.HasSeries)
        //{
        //    var comicBelongedSeries = comicDetail.ArtworkSeries;

        //    responseDto.SeriesId = comicBelongedSeries.SeriesId.ToString();
        //    responseDto.SeriesName = comicBelongedSeries.Series.Title;
        //}

        responseDto.Categories = comicDetail.ArtworkCategories.Select(category => new CategoryDto
        {
            CategoryId = category.Id.ToString(),
            CategoryName = category.Name,
        });

        return responseDto;
    }
}
