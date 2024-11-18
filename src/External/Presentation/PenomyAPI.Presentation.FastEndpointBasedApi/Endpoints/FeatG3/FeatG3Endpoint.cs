using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG3;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G3Endpoint : EndpointWithoutRequest<FeatG3HttpResponse>
{
    public override void Configure()
    {
        Get("g3/recently-updated/comics");
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

    public override async Task<FeatG3HttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var featG3Request = new FeatG3Request();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<FeatG3Request, FeatG3Response>(
            featG3Request,
            ct
        );

        var httpResponse = G3HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG3Request, featResponse);

        return httpResponse;
    }
}
