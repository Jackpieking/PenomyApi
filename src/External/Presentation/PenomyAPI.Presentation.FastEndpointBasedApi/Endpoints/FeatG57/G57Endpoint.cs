using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG57;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57;

public class G57Endpoint : Endpoint<G57Request, G57HttpResponse>
{
    public override void Configure()
    {
        Post("/g57/comment/unlike/");
        PreProcessor<G57PreProcessor>();
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

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

    public override async Task<G57HttpResponse> ExecuteAsync(G57Request req, CancellationToken ct)
    {
        var stateBag = ProcessorState<G57StateBag>();

        req.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G57Request, G57Response>(req, ct);

        var httpResponse = G57HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G57ResponseDto { IsSuccess = featResponse.IsSuccess };

            return httpResponse;
        }

        return httpResponse;
    }
}
