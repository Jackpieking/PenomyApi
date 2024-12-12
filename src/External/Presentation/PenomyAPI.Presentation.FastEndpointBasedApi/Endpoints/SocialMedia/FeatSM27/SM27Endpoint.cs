using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM27;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27;

public class SM27Endpoint : Endpoint<SM27RequestDto, SM27HttpResponse>
{
    public override void Configure()
    {
        Post("sm27/post-comment/take-down");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM27AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for taking down post comment.";
            summary.Description = "This endpoint is used for taking down post comment.";
            summary.Response<SM27HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM27ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM27HttpResponse> ExecuteAsync(
        SM27RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<SM27StateBag>();

        var SM27Request = new SM27Request
        {
            CommentId = long.Parse(req.CommentId),
            PostId = long.Parse(req.PostId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM27Request, SM27Response>(
            SM27Request,
            ct
        );

        var httpResponse = SM27HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(SM27Request, featResponse);

        return httpResponse;
    }
}
