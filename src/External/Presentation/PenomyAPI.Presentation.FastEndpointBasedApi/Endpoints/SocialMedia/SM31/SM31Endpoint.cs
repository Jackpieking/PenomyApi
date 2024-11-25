using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM31;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM31.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM31.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM31.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM31;

public class SM31Endpoint
    : Endpoint<SM31RequestDto, SM31HttpResponse>
{
    public override void Configure()
    {
        Get("/SM31/friend/remove");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM31RequestDto>>();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user to send unfriend request";
            summary.Description = "This endpoint is used for user send to send unfriend request";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM31HttpResponse { AppCode = SM31ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM31HttpResponse> ExecuteAsync(
        SM31RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM31Request
        {
            UserId = stateBag.AppRequest.UserId,
            FriendId = requestDto.FriendId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM31Request, SM31Response>(
            featRequest,
            ct
        );

        var httpResponse = SM31ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
