using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG28;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28;

public class G28AuthEndpoint : Endpoint<G28Request, G28HttpResponse>
{
    public override void Configure()
    {
        Get("g28/creator/artworks");
        AllowAnonymous();

        DontThrowIfValidationFails();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting created artworks of the specified creator.";
            summary.Description =
                "This endpoint is used for getting created artworks of the specified creator";
            summary.Response<G28HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G28ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G28HttpResponse> ExecuteAsync(
        G28Request request,
        CancellationToken ct
    )
    {
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G28Request, G28Response>(
            request,
            ct
        );

        var httpResponse = G28HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(request, featResponse);

        return httpResponse;
    }
}
