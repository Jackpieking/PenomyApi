using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.Middlewares.Authorization;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8;

public class Art8Endpoint : Endpoint<Art8RequestDto, Art8HttpResponse>
{
    public override void Configure()
    {
        Delete("art8/temp-remove/{artworkId}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<Art8AuthorizationPreProcessor>();
    }

    public override async Task<Art8HttpResponse> ExecuteAsync(
        Art8RequestDto requestDto,
        CancellationToken ct)
    {
        var user = User;

        var creatorId = 123456789012345678;
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
