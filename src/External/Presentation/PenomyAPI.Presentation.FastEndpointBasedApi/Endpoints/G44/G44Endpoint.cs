using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G44;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44;

public class G44Endpoint : Endpoint<G44Request, G44HttpResponse>
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
            summary.Response<G44HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G44ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G44HttpResponse> ExecuteAsync(
        G44Request requestDto,
        CancellationToken ct
    )
    {
        var featRequest = new G44Request
        {
            userId = requestDto.userId,
            artworkId = requestDto.artworkId,
            ArtworkType = requestDto.ArtworkType,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G44Request, G44Response>(
            featRequest,
            ct
        );

        var httpResponse = G44ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G44ResponseDto { Isuccess = true };
            return httpResponse;
        }

        return httpResponse;
    }
}
