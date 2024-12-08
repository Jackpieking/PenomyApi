using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM25;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25;

public class SM25Endpoint : Endpoint<SM25RequestDto, SM25HttpResponse>
{
    public override void Configure()
    {
        Post("sm25/post-comment/update");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM25AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for editting post comment.";
            summary.Description = "This endpoint is used for editting post comment.";
            summary.Response<SM25HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM25HttpResponse> ExecuteAsync(
        SM25RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<SM25StateBag>();

        var SM25Request = new SM25Request
        {
            CommentId = long.Parse(req.CommentId),
            NewComment = req.CommentContent,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM25Request, SM25Response>(
            SM25Request,
            ct
        );

        var httpResponse = SM25HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(SM25Request, featResponse);

        return httpResponse;
    }
}
