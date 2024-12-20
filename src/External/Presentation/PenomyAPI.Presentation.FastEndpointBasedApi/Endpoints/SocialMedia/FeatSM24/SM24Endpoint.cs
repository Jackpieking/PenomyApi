using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM24;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24;

public class SM24Endpoint : Endpoint<SM24RequestDto, SM24HttpResponse>
{
    public override void Configure()
    {
        Post("sm24/post-comment/create");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM24AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating post comment.";
            summary.Description = "This endpoint is used for creating post comment.";
            summary.Response<SM24HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM24ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM24HttpResponse> ExecuteAsync(
        SM24RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<SM24StateBag>();

        var SM24Request = new SM24Request
        {
            Comment = req.CommentContent,
            PostId = long.Parse(req.PostId),
            IsGroupPostComment = req.IsGroupPostComment,
        };
        SM24Request.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM24Request, SM24Response>(
            SM24Request,
            ct
        );

        var httpResponse = SM24HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(SM24Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM24ResponseDto { CommentId = featResponse.CommentId };

            return httpResponse;
        }

        return httpResponse;
    }
}
