using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadPublicLevel;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4;

public sealed class Art7LoadPublicLevelEndpoint
    : EndpointWithoutRequest<Art7LoadPublicLevelHttpResponse>
{
    public override void Configure()
    {
        Get("art7/public-levels");

        AllowAnonymous();
    }

    public override async Task<Art7LoadPublicLevelHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art7LoadPublicLevelRequest,
            Art7LoadPublicLevelResponse
        >(request: Art7LoadPublicLevelRequest.Empty, ct: ct);

        var publicLevelDtos = featResponse
            .PublicLevels.Where(publicLevel =>
                publicLevel != ArtworkPublicLevel.PrivateWithLimitedUsers
                && publicLevel != ArtworkPublicLevel.OnlyFriend
            )
            .Select(selector: publicLevel => PublicLevelDto.CovertToDto(publicLevel));

        var httpResponse = new Art7LoadPublicLevelHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = publicLevelDtos,
        };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
