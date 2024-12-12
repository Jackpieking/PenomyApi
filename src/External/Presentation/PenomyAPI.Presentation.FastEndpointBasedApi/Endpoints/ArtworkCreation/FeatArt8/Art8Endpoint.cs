using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8;

public class Art8Endpoint : Endpoint<Art8RequestDto, Art8HttpResponse>
{
    public override void Configure()
    {
        Delete("art8/temp-remove/{artworkId}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art8RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art8RequestDto>>();
    }

    public override async Task<Art8HttpResponse> ExecuteAsync(
        Art8RequestDto requestDto,
        CancellationToken ct)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapToRequest(creatorId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art8Request, Art8Response>(
            request,
            ct);

        var t = HttpContext;

        var httpResponse = Art8HttpResponseMapper
            .Resolve(featureResponse.AppCode)
            .Invoke(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
