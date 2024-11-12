using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG49;
using PenomyAPI.App.FeatG49.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49;

public class G49GetStarRateEndpoint : Endpoint<G49GetStarRateRequestDto, G49GetStarRateHttpResponse>
{
    public override void Configure()
    {
        Post("/g49/artwork/get-rate");
        PreProcessor<AuthPreProcessor<G49GetStarRateRequestDto>>();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        DontThrowIfValidationFails();
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for add to get user artwork rate";
            summary.Description = "This endpoint is used for getting user artwork rate";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G49HttpResponse(G49ResponseStatusCode.SUCCESS.ToString()));
        });
    }

    public override async Task<G49GetStarRateHttpResponse> ExecuteAsync(
        G49GetStarRateRequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();
        var g49Req = new G49StarRateRequest
        {
            ArtworkId = requestDto.ArtworkId,
            UserId = stateBag.AppRequest.UserId
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<G49StarRateRequest, G49StarRateResponse>(g49Req, ct);

        var httpResponse = G49GetStarRateResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g49Req, featResponse);
        if (featResponse.AppCode is G49ResponseStatusCode.SUCCESS)
            httpResponse.Body = new G49GetStarRateResponseDto
            {
                CurrentUserRate = featResponse.StarRate
            };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
