using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadPublicLevels;

public sealed class Art4LoadPublicLevelHandler
    : IFeatureHandler<Art4LoadPublicLevelRequest, Art4LoadPublicLevelResponse>
{
    public Task<Art4LoadPublicLevelResponse> ExecuteAsync(
        Art4LoadPublicLevelRequest request,
        CancellationToken ct
    )
    {
        var publicLevels = Enum.GetValues<ArtworkPublicLevel>();

        var response = new Art4LoadPublicLevelResponse { PublicLevels = publicLevels, };

        return Task.FromResult(response);
    }
}
