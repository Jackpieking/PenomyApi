using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadArtworkStatus;

public sealed class Art7LoadArtworkStatusHandler
    : IFeatureHandler<Art7LoadArtworkStatusRequest, Art7LoadArtworkStatusResponse>
{
    public Task<Art7LoadArtworkStatusResponse> ExecuteAsync(
        Art7LoadArtworkStatusRequest request,
        CancellationToken ct)
    {
        var artworkStatuses = Enum.GetValues<ArtworkStatus>();

        var response = new Art7LoadArtworkStatusResponse
        {
            ArtworkStatuses = artworkStatuses,
        };

        return Task.FromResult(response);
    }
}
