using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G61;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61;

public class G61Endpoint : Endpoint<G61Request, G61HttpResponse>
{
    public override void Configure()
    {
        Post("/G61/favorite-artworks");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user follows a creator";
            summary.Description = "This endpoint is used for user to follows a creator";
            summary.Response<G61HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G61ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G61HttpResponse> ExecuteAsync(
        G61Request requestDto,
        CancellationToken ct
    )
    {
        var featRequest = new G61Request
        {
            UserId = requestDto.UserId,
            CreatorId = requestDto.CreatorId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G61Request, G61Response>(
            featRequest,
            ct
        );

        var httpResponse = G61ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G61ResponseDto { Isuccess = true };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
