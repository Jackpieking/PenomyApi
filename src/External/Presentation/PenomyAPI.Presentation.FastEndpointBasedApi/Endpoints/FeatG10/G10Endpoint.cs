using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG10;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G10Endpoint : Endpoint<G10Request, G10HttpResponse>
{
    public override void Configure()
    {
        Get("/g10/ArtworkComment/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting artwork comment.";
            summary.Description = "This endpoint is used for getting artwork comment.";
            summary.Response<G10HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G10ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G10HttpResponse> ExecuteAsync(G10Request req, CancellationToken ct)
    {
        var featG10Request = new G10Request
        {
            ArtworkId = req.ArtworkId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G10Request, G10Response>(req, ct);

        var httpResponse = G10HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG10Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G10ResponseDto
            {
                ArtworkList = featResponse.Result.ToList()
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
