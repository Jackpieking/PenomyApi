using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM30.SM30UnsendHandler;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30Unsend.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30Unsend.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30UnsendUnsend.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30UnsendUnsend;

public class SM30UnsendUnsendEndpoint : Endpoint<SM30UnsendRequestDto, SM30UnsendHttpResponse>
{
    public override void Configure()
    {
        Post("/SM30/friend-request/remove");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM30UnsendRequestDto>>();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user unsend friend request";
            summary.Description = "This endpoint is used for user unsend friend request";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM30UnsendHttpResponse
                {
                    AppCode = SM30UnsendResponseStatusCode.SUCCESS.ToString()
                }
            );
        });
    }

    public override async Task<SM30UnsendHttpResponse> ExecuteAsync(
        SM30UnsendRequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM30UnsendRequest
        {
            UserId = stateBag.AppRequest.UserId,
            FriendId = long.Parse(requestDto.FriendId)
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM30UnsendRequest, SM30UnsendResponse>(
            featRequest,
            ct
        );

        var httpResponse = SM30UnsendHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
