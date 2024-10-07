using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG53;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53;

public class G53Endpoint : Endpoint<G53Request, G53HttpResponse>
{
    public override void Configure()
    {
        Put("G53/comment/edit");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating artwork comment.";
            summary.Description = "This endpoint is used for creating artwork comment.";
            summary.Response<G53HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G53ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G53HttpResponse> ExecuteAsync(G53Request req, CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<G53Request, G53Response>(req, ct);

        var httpResponse = G53HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G53Response
            {
                IsSuccess = featResponse.IsSuccess,
                StatusCode = G53ResponseStatusCode.SUCCESS
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
