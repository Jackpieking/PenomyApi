using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G43;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43;

public class G43Endpoint : Endpoint<G43Request, G43HttpResponse>
{
    public override void Configure()
    {
        Post("/profile/user/followed-artworks");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user follow artwork";
            summary.Description = "This endpoint is used for user follow artwork";
            summary.Response<G43HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G43ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G43HttpResponse> ExecuteAsync(
        G43Request requestDto,
        CancellationToken ct
    )
    {
        var featRequest = new G43Request
        {
            UserId = requestDto.UserId,
            ArtworkId = requestDto.ArtworkId,
            ArtworkType = requestDto.ArtworkType,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G43Request, G43Response>(
            featRequest,
            ct
        );

        var httpResponse = G43ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G43ResponseDto { Isuccess = true };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
