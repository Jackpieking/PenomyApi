using PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.DTOs;

public class Art17AnimeDetailResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string OriginId { get; set; }

    public string Introduction { get; set; }

    public string ThumbnailUrl { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public IEnumerable<Art17ArtworkCategoryDto> SelectedCategories { get; set; }

    public bool AllowComment { get; set; }

    public static Art17AnimeDetailResponseDto MapFrom(Art17GetAnimeDetailResponse response)
    {
        var animeDetail = response.AnimeDetail;

        return new()
        {
            Id = animeDetail.Id.ToString(),
            Title = animeDetail.Title,
            ArtworkStatus = animeDetail.ArtworkStatus,
            Introduction = animeDetail.Introduction,
            OriginId = animeDetail.ArtworkOriginId.ToString(),
            ThumbnailUrl = animeDetail.ThumbnailUrl,
            PublicLevel = animeDetail.PublicLevel,
            SelectedCategories = animeDetail.ArtworkCategories.Select(category => new Art17ArtworkCategoryDto
            {
                CategoryId = category.CategoryId.ToString(),
            }),
            AllowComment = animeDetail.AllowComment,
            UpdatedAt = animeDetail.UpdatedAt,
        };
    }

    public class Art17ArtworkCategoryDto
    {
        public string CategoryId { get; set; }
    }
}
