using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG56;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56;

public class G56Endpoint : Endpoint<G56RequestDto, G56HttpResponse>
{
    public override void Configure()
    {
        Post("/g56/ArtworkComment/like/");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for deleting artwork comment.";
            summary.Description = "This endpoint is used for deleting artwork comment.";
            summary.Response<G56HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G56ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G56HttpResponse> ExecuteAsync(
        G56RequestDto req,
        CancellationToken ct
    )
    {
        var G56Request = new G56Request
        {
            CommentId = long.Parse(req.CommentId),
            UserId = long.Parse(req.UserId),
        };
        
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G56Request, G56Response>(
            G56Request,
            ct
        );

        var httpResponse = G56HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G56Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G56ResponseDto { IsSuccess = featResponse.IsSuccess };

            return httpResponse;
        }

        return httpResponse;
    }
}
