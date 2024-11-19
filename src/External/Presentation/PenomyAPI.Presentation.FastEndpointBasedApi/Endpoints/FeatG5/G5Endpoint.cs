using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Middlewares;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5;

public class G5Endpoint : Endpoint<G5RequestDto, G5HttpResponse>
{
    public override void Configure()
    {
        Get("/g5/artwork-detail");

        AllowAnonymous();
        PreProcessor<G5AuthPreProcessor>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get artwork detail";
            summary.Description = "This endpoint is used for get artwork detail";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G5HttpResponse { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G5HttpResponse> ExecuteAsync(
        G5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G5StateBag>();

        var g5Req = new G5Request
        {
            ForSignedInUser = stateBag.IsAuthenticated,
            UserId = stateBag.UserId,
            ComicId = requestDto.ArtworkId,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G5Request, G5Response>(g5Req, ct);

        var httpResponse = G5HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g5Req, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
