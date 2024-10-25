using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG57;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57;

public class G57Endpoint : Endpoint<G57RequestDto, G57HttpResponse>
{
    public override void Configure()
    {
        Post("/g57/comment/unlike/");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for unlike artwork comment.";
            summary.Description = "This endpoint is used for unlike artwork comment.";
            summary.Response<G57HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G57ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G57HttpResponse> ExecuteAsync(
        G57RequestDto req,
        CancellationToken ct
    )
    {
        var G57Request = new G57Request
        {
            CommentId = long.Parse(req.CommentId),
            UserId = long.Parse(req.UserId),
        };
        
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G57Request, G57Response>(
            G57Request,
            ct
        );

        var httpResponse = G57HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G57Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G57ResponseDto { IsSuccess = featResponse.IsSuccess };

            return httpResponse;
        }

        return httpResponse;
    }
}
