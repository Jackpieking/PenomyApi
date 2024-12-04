using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.APP.FeatG6;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6;

public class G6Endpoint : Endpoint<G6Request, G6HttpResponse>
{
    public override void Configure()
    {
        Get("g6/artworks/recommend");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary =
                "Endpoint for get list of artworks have the same series as current artwork";
            summary.Description =
                "This endpoint is used for get list of artworks have the same series as current artwork";
            summary.Response<G6HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G6ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G6HttpResponse> ExecuteAsync(G6Request request, CancellationToken ct)
    {
        var httpResponse = new G6HttpResponse();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G6Request, G6Response>(request, ct);

        httpResponse = G6HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(request, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
