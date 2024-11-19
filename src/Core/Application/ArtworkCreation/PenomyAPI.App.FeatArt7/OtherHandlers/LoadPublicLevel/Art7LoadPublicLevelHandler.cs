using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadPublicLevel;

public sealed class Art7LoadPublicLevelHandler
    : IFeatureHandler<Art7LoadPublicLevelRequest, Art7LoadPublicLevelResponse>
{
    public Task<Art7LoadPublicLevelResponse> ExecuteAsync(
        Art7LoadPublicLevelRequest request,
        CancellationToken ct
    )
    {
        var publicLevels = Enum.GetValues<ArtworkPublicLevel>();

        var response = new Art7LoadPublicLevelResponse { PublicLevels = publicLevels, };

        return Task.FromResult(response);
    }
}
