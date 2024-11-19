using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG52;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52;

public class G52Endpoint : Endpoint<G52RequestDto, G52HttpResponse>
{
    public override void Configure()
    {
        Post("g52/comment/create");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G52AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating artwork comment.";
            summary.Description = "This endpoint is used for creating artwork comment.";
            summary.Response<G52HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G52ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G52HttpResponse> ExecuteAsync(
        G52RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G52StateBag>();

        var _artworkComment = new ArtworkComment
        {
            Content = req.CommentContent,
            ArtworkId = long.Parse(req.ArtworkId),
            ChapterId = long.Parse(req.ChapterId),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = long.Parse(stateBag.AppRequest.GetUserId()),
        };
        var G52Request = new G52Request { ArtworkComment = _artworkComment };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G52Request, G52Response>(
            G52Request,
            ct
        );

        var httpResponse = G52HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G52Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G52ResponseDto { CommentId = featResponse.CommentId };

            return httpResponse;
        }

        return httpResponse;
    }
}
