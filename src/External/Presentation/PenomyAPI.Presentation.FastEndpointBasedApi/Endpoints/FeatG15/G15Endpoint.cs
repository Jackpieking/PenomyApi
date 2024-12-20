using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG15;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG15.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG115;

public class G15Endpoint : Endpoint<G15Request, G15HttpResponse>
{
    public override void Configure()
    {
        Get("g15/anime-detail");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get anime detail";
            summary.Description = "This endpoint is used for get anime detail";
            summary.Response<G15HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G15ResponseAppCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G15HttpResponse> ExecuteAsync(
        G15Request request,
        CancellationToken ct
    )
    {
        var httpResponse = new G15HttpResponse();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G15Request, G15Response>(
            request,
            ct
        );

        httpResponse = G15HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(request, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
