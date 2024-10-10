using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4;

public class G54Endpoint : Endpoint<G4RequestDto, G4HttpResponse>
{
    public override void Configure()
    {
        Get("g4/ComicsByCategory/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting comics by category.";
            summary.Description = "This endpoint is used for getting comics by category.";
            summary.Response<G4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G4HttpResponse> ExecuteAsync(G4RequestDto req, CancellationToken ct)
    {
        var G4Request = new G4Request { Category = req.CategoryId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G4Request, G4Response>(
            G4Request,
            ct
        );

        var httpResponse = G4HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G4Request, featResponse);

        httpResponse.Body = new G4ResponseDto { ArtworkList = featResponse.Result };

        return httpResponse;
    }
}
