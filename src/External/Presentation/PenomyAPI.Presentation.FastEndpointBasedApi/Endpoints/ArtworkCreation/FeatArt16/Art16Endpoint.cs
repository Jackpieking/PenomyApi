using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt16;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16;

public class Art16Endpoint : Endpoint<Art16RequestDto, Art16HttpResponse>
{
    public override void Configure()
    {
        Get("art16/anime/detail/{artworkId}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art16RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art16RequestDto>>();
    }

    public override async Task<Art16HttpResponse> ExecuteAsync(
        Art16RequestDto requestDto,
        CancellationToken ct)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapToRequest(creatorId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art16Request, Art16Response>(
            request,
            ct);

        var httpResponse = Art16HttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
