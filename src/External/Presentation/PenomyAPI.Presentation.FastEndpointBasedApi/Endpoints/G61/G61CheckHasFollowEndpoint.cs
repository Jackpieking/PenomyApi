using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G61;
using PenomyAPI.App.G61.OtherHandlers.CheckHasFollow;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Middlewares;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61;

public class G61CheckHasFollowEndpoint : Endpoint<G61CheckHasFollowRequestDto, G61CheckHasFollowHttpResponse>
{
    public override void Configure()
    {
        Post("g61/check-follow");

        AllowAnonymous();
        PreProcessor<G61CheckHasFollowedPreProcessor>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user to check if they has followed the creator";
            summary.Description = "This endpoint is used for user to check if they has followed the creator";
            summary.Response<G61HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G61ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G61CheckHasFollowHttpResponse> ExecuteAsync(
        G61CheckHasFollowRequestDto requestDto, CancellationToken ct)
    {
        var stateBag = ProcessorState<G61StateBag>();

        G61CheckHasFollowHttpResponse httpResponse;

        if (!stateBag.IsAuthenticated)
        {
            httpResponse = G61CheckHasFollowHttpResponse.HAS_NOT_FOLLOWED;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        var request = new G61CheckHasFollowRequest
        {
            CreatorId = requestDto.CreatorId,
            UserId = stateBag.UserId,
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G61CheckHasFollowRequest, G61CheckHasFollowResponse>(request, ct);

        httpResponse = new G61CheckHasFollowHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new()
            {
                HasFollowed = featureResponse.HasFollowed,
            }
        };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
