using System;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;

public sealed class ArtworkDetailResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public string OriginImageUrl { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public long TotalUsersRated { get; set; }

    public double AverageStarRate { get; set; }

    public int TotalChapters { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
