using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadPublicLevels;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4;

public sealed class Art4LoadPublicLevelEndpoint
    : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("art4/public-levels");

        AllowAnonymous();

    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<Art4LoadPublicLevelRequest, Art4LoadPublicLevelResponse>(
            request: Art4LoadPublicLevelRequest.Empty,
            ct: ct);

        var publicLevelDtos = featResponse.PublicLevels
            .Where(publicLevel =>
                publicLevel != ArtworkPublicLevel.PrivateWithLimitedUsers
                && publicLevel != ArtworkPublicLevel.OnlyFriend)
            .Select(selector: publicLevel => PublicLevelDto.CovertToDto(publicLevel));

        var responseBody = new Art4LoadPublicLevelHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = publicLevelDtos,
        };

        return Results.Ok(responseBody);
    }
}
