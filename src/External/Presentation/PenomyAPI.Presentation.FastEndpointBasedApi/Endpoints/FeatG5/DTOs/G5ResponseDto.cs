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
    public string CreatorName { get; set; }

    [ProtoMember(6)]
    public string CreatorAvatarUrl { get; set; }

    [ProtoMember(7)]
    public long CreatorTotalFollowers { get; set; }

    [ProtoMember(8)]
    public string CountryId { get; set; }

    [ProtoMember(9)]
    public string CountryName { get; set; }

    [ProtoMember(10)]
    public IEnumerable<CategoryDto> Categories { get; set; }

    [ProtoMember(11)]
    public ArtworkStatus ArtworkStatus { get; set; }

    [ProtoMember(12)]
    public string SeriesId { get; set; }

    [ProtoMember(13)]
    public string SeriesName { get; set; }

    [ProtoMember(14)]
    public long ViewCount { get; set; }

    [ProtoMember(15)]
    public long FavoriteCount { get; set; }

    [ProtoMember(16)]
    public double StarRates { get; set; }

    [ProtoMember(17)]
    public long TotalUsersRated { get; set; }

    [ProtoMember(18)]
    public string ThumbnailUrl { get; set; }

    [ProtoMember(19)]
    public string Introduction { get; set; }

    [ProtoMember(20)]
    public long CommentCount { get; set; }

    [ProtoMember(21)]
    public long FollowCount { get; set; }

    [ProtoMember(22)]
    public bool IsUserFavorite { get; set; }

    [ProtoMember(23)]
    public bool HasFollowed { get; set; }

    [ProtoMember(24)]
    public bool IsAllowComment { get; set; }

    [ProtoMember(25)]
    public string FirstChapterId { get; set; }

    [ProtoMember(26)]
    public string LastReadChapterId { get; set; }

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
            StarRates = comicDetail.ArtworkMetaData.GetAverageStarRate(),
            TotalUsersRated = comicDetail.ArtworkMetaData.TotalUsersRated,
            ViewCount = comicDetail.ArtworkMetaData.TotalViews,
            FavoriteCount = comicDetail.ArtworkMetaData.TotalFavorites,
            CommentCount = comicDetail.ArtworkMetaData.TotalComments,
            FollowCount = comicDetail.ArtworkMetaData.TotalFollowers,
            IsAllowComment = comicDetail.AllowComment,
            IsUserFavorite = featResponse.IsUserFavorite,
            HasFollowed = featResponse.HasFollowed,
            // Chapter View History section.
            FirstChapterId = comicDetail.FirstChapterId.ToString(),
            LastReadChapterId = comicDetail.LastReadChapterId.ToString(),
            // Creator detail section.
            CreatorId = comicDetail.CreatorId.ToString(),
            CreatorName = comicDetail.CreatorName,
            CreatorAvatarUrl = comicDetail.CreatorAvatarUrl,
            CreatorTotalFollowers = comicDetail.CreatorTotalFollowers,
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
