using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG3;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G3Endpoint : Endpoint<FeatG3Request, G3HttpResponse>
{
    public override void Configure()
    {
        Get("/g3/RecentlyUpdatedComics/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting recently updated comic.";
            summary.Description = "This endpoint is used for getting recently updated comic.";
            summary.Response<G3HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = FeatG3ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G3HttpResponse> ExecuteAsync(FeatG3Request req, CancellationToken ct)
    {
        var featG3Request = new FeatG3Request
        {
            empty = req.empty
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<FeatG3Request, FeatG3Response>(featG3Request, ct);

        var httpResponse = G3HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG3Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G3ResponseDto
            {
                ArtworkList = featResponse.Result.ToList()
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
