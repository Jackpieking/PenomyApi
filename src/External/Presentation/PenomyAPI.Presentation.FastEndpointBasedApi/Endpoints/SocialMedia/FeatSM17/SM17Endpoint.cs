using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Core;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM17;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17;

public class SM17Endpoint : Endpoint<SM17RequestDto, SM17HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static SM17Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("sm17/like-unlike-posts");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM17AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating or remove like of a post.";
            summary.Description = "This endpoint is used for creating or remove like of a post.";
            summary.Response<SM17HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM17ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM17HttpResponse> ExecuteAsync(
        SM17RequestDto req,
        CancellationToken ct
    )
    {
        SM17HttpResponse httpResponse;

        var stateBag = ProcessorState<SM17StateBag>();

        var featRequest = new SM17Request
        {
            PostId = long.Parse(req.PostId),
            UserId = long.Parse(stateBag.AppRequest.GetUserId()),
            IsGroupPost = req.IsGroupPost,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM17Request, SM17Response>(
            featRequest,
            ct
        );

        httpResponse = SM17HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        httpResponse.Body = featResponse;

        return httpResponse;
    }
}
