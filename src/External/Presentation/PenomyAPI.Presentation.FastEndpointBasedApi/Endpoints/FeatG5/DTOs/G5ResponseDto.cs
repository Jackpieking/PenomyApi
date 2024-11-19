using PenomyAPI.App.FeatG5;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;

public class G5ResponseDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public bool HasSeries { get; set; }

    public string AuthorId { get; set; }

    public string AuthorName { get; set; }

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

    public static G5ResponseDto MapFrom(G5Response featResponse)
    {
        var comicDetail = featResponse.Result;

        var responseDto = new G5ResponseDto
        {
            Id = comicDetail.Id,
            Title = comicDetail.Title,
            ThumbnailUrl = comicDetail.ThumbnailUrl,
            Introduction = comicDetail.Introduction,
            AuthorId = comicDetail.Creator.UserId.ToString(),
            AuthorName = comicDetail.Creator.NickName,
            CountryName = comicDetail.Origin.CountryName,
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
        };

        if (comicDetail.HasSeries)
        {
            var comicBelongedSeries = comicDetail.ArtworkSeries
                .Select(belongedSeries => belongedSeries.Series)
                .FirstOrDefault();

            responseDto.SeriesId = comicBelongedSeries.Id.ToString();
            responseDto.SeriesName = comicBelongedSeries.Title;
        }

        responseDto.Categories = comicDetail.ArtworkCategories
            .Select(cate => new CategoryDto
            {
                CategoryId = cate.Category.Id.ToString(),
                CategoryName = cate.Category.Name
            });

        return responseDto;
    }
}
