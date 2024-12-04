using System.Collections.Generic;
using System.Linq;
using PenomyAPI.App.FeatG5;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;

public class G5ResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public bool HasSeries { get; set; }

    public string CreatorId { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatarUrl { get; set; }

    public long CreatorTotalFollowers { get; set; }

    public string CountryId { get; set; }

    public string CountryName { get; set; }

    public IEnumerable<CategoryDto> Categories { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public string SeriesId { get; set; }

    public string SeriesName { get; set; }

    public long ViewCount { get; set; }

    public long FavoriteCount { get; set; }

    public double StarRates { get; set; }

    public long TotalUsersRated { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    public long CommentCount { get; set; }

    public long FollowCount { get; set; }

    public bool IsUserFavorite { get; set; }

    public bool HasFollowed { get; set; }

    public bool IsAllowComment { get; set; }

    public string FirstChapterId { get; set; }

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
