using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG3;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G3Endpoint : Endpoint<FeatG3Request, FeatG3HttpResponse>
{
    public override void Configure()
    {
        Get("g3/recently-updated/artworks");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting recently updated comic.";
            summary.Description = "This endpoint is used for getting recently updated comic.";
            summary.Response<FeatG3HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = FeatG3ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<FeatG3HttpResponse> ExecuteAsync(FeatG3Request request, CancellationToken ct)
    {
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<FeatG3Request, FeatG3Response>(
            request,
            ct
        );

        var httpResponse = G3HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(request, featResponse);

        return httpResponse;
    }
}
