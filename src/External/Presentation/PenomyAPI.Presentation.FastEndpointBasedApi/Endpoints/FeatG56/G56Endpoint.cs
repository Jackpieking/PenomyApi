using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG56;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56;

public class G56Endpoint : Endpoint<G56Request, G56HttpResponse>
{
    public override void Configure()
    {
        Post("/g56/ArtworkComment/like/");
        PreProcessor<G56PreProcessor>();
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for like artwork comment.";
            summary.Description = "This endpoint is used for like artwork comment.";
            summary.Response<G56HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G56ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G56HttpResponse> ExecuteAsync(
        G56Request req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G56StateBag>();

        req.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G56Request, G56Response>(
            req,
            ct
        );

        var httpResponse = G56HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G56ResponseDto { IsSuccess = featResponse.IsSuccess };

            return httpResponse;
        }

        return httpResponse;
    }
}
