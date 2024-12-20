using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;

public class Art3DeletedArtworkItemResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public long TotalUsersRated { get; set; }

    public double AverageStarRates { get; set; }

    public int TotalChapters { get; set; }

    public DateTime RemovedAt { get; set; }

    public long TotalViews { get; set; }

    public long TotalFollowers { get; set; }

    public static Art3DeletedArtworkItemResponseDto MapFrom(
        Art3DeletedArtworkDetailReadModel model)
    {
        return new()
        {
            Id = model.Id.ToString(),
            Title = model.Title,
            PublicLevel = model.PublicLevel,
            ArtworkStatus = model.ArtworkStatus,
            TotalUsersRated = model.TotalUsersRated,
            AverageStarRates = model.GetAverageStarRates(),
            RemovedAt = model.RemovedAt,
            ThumbnailUrl = model.ThumbnailUrl,
            TotalChapters = model.TotalChapters,
            TotalFollowers = model.TotalFollowers,
            TotalViews = model.TotalViews,
        };
    }
}
