using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;

namespace PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;

public sealed class Art3DeletedArtworkDetailReadModel
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public long TotalStarRates { get; set; }

    public long TotalUsersRated { get; set; }

    public int TotalChapters { get; set; }

    public DateTime RemovedAt { get; set; }

    public long TotalViews { get; set; }

    public long TotalFollowers { get; set; }

    public double GetAverageStarRates()
        => ArtworkMetaData.GetAverageStarRate(TotalStarRates, TotalUsersRated);
}
