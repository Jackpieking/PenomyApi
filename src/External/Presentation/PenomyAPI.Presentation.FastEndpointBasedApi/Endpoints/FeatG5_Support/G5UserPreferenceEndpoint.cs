using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support;

public class G5UserPreferenceEndpoint : Endpoint<G5RequestDto, G5UserPreferenceHttpResponse>
{
    public override void Configure()
    {
        Get("g5/user-preference");

        AllowAnonymous();
        PreProcessor<G5AuthPreProcessor>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get current user preference";
            summary.Description = "This endpoint is used for get current user preference";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G5UserPreferenceHttpResponse { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G5UserPreferenceHttpResponse> ExecuteAsync(
        G5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G5StateBag>();

        G5UserPreferenceRequest featRequest;

        if (stateBag.IsAuthenticated)
        {
            featRequest = G5UserPreferenceRequest.ForUser(
                stateBag.UserId,
                requestDto.ArtworkId);
        }
        else
        {
            featRequest = G5UserPreferenceRequest.ForUser(
                requestDto.GuestId,
                requestDto.ArtworkId);
        }

        var response = await FeatureExtensions
            .ExecuteAsync<G5UserPreferenceRequest, G5UserPreferenceResponse>(featRequest, ct);

        var httpResponse = G5UserPreferenceHttpResponse.MapFrom(response);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
