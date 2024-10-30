using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG58;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58;

public class G58Endpoint : Endpoint<G58RequestDto, G58HttpResponse>
{
    public override void Configure()
    {
        Post("g58/replycomment/create");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G58AuthorizationPreProcessor>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating artwork reply comment.";
            summary.Description = "This endpoint is used for creating artwork reply comment.";
            summary.Response<G58HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G58ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G58HttpResponse> ExecuteAsync(
        G58RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G58StateBag>();

        var _artworkComment = new ArtworkComment
        {
            Content = req.CommentContent,
            ArtworkId = long.Parse(req.ArtworkId),
            ChapterId = long.Parse(req.ChapterId),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = long.Parse(stateBag.AppRequest.GetUserId()),
        };
        var G58Request = new G58Request
        {
            ReplyComment = _artworkComment,
            ParentCommentId = long.Parse(req.ParentCommentId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G58Request, G58Response>(
            G58Request,
            ct
        );

        var httpResponse = G58HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G58Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G58ResponseDto { CommentId = featResponse.CommentId };

            return httpResponse;
        }

        return httpResponse;
    }
}
