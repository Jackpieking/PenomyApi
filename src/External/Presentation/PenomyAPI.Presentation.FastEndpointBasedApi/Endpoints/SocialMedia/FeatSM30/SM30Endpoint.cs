using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM30;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM30.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.FeatSM30.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.FeatSM30.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.FeatSM30;

public class SM30Endpoint : Endpoint<SM30RequestDto, SM30HttpResponse>
{
    public override void Configure()
    {
        Post("/SM30/friend/add");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM30RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user send friend request";
            summary.Description = "This endpoint is used for user send friend request";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM30HttpResponse
                {
                    AppCode = SM30ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<SM30HttpResponse> ExecuteAsync(
        SM30RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM30Request
        {
            UserId = stateBag.AppRequest.UserId,
            FriendId = requestDto.FriendId,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM30Request, SM30Response>(
            featRequest,
            ct
        );

        var httpResponse = SM30ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
