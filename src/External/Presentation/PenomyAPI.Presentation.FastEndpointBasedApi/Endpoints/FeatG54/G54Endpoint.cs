using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG54;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG54.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG54.HttpResponse;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG54;

public class G54Endpoint : Endpoint<G54Request, G54HttpResponse>
{
    public override void Configure()
    {
        Delete("/g54/ArtworkComment/delete");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for deleting artwork comment.";
            summary.Description = "This endpoint is used for deleting artwork comment.";
            summary.Response<G54HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G54ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G54HttpResponse> ExecuteAsync(G54Request req, CancellationToken ct)
    {
        var G54Request = new G54Request
        {
            ArtworkCommentId = req.ArtworkCommentId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G54Request, G54Response>(G54Request, ct);

        var httpResponse = G54HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G54Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G54ResponseDto
            {
                IsSuccess = featResponse.IsSuccess
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
