using PenomyAPI.App.FeatG15;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG15.DTOs;

public class G15ResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public bool HasSeries { get; set; }

    public string AuthorName { get; set; }

    public string CountryName { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public string SeriesName { get; set; }

    public long ViewCount { get; set; }

    public long FavoriteCount { get; set; }

    public double StarRates { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    public long CommentCount { get; set; }

    public string CreatorId { get; set; }

    public string CountryId { get; set; }

    public IEnumerable<CategoryDto> Categories { get; set; }

    public string SeriesId { get; set; }

    public long TotalUsersRated { get; set; }

    public long FollowCount { get; set; }

    public bool IsAllowComment { get; set; }

    public static G15ResponseDto MapFrom(G15Response response)
    {
        var animeDetail = response.Result;

        var responseDto = new G15ResponseDto()
        {
            Id = animeDetail.Id.ToString(),
            Title = animeDetail.Title,
            ThumbnailUrl = animeDetail.ThumbnailUrl,
            Introduction = animeDetail.Introduction,
            CountryId = animeDetail.CountryId.ToString(),
            CountryName = animeDetail.CountryName,
            HasSeries = animeDetail.HasSeries,
            ArtworkStatus = animeDetail.ArtworkStatus,
            StarRates = animeDetail.ArtworkMetaData.GetAverageStarRate(),
            TotalUsersRated = animeDetail.ArtworkMetaData.TotalUsersRated,
            ViewCount = animeDetail.ArtworkMetaData.TotalViews,
            FavoriteCount = animeDetail.ArtworkMetaData.TotalFavorites,
            CommentCount = animeDetail.ArtworkMetaData.TotalComments,
            FollowCount = animeDetail.ArtworkMetaData.TotalFollowers,
            IsAllowComment = animeDetail.AllowComment,
            // Creator detail section.
            CreatorId = animeDetail.CreatorId.ToString(),
        };

        responseDto.Categories = animeDetail.ArtworkCategories.Select(category => new CategoryDto
        {
            CategoryId = category.Id.ToString(),
            CategoryName = category.Name,
        });

        return responseDto;
    }
}
