using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG53;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53;

public class G53Endpoint : Endpoint<G53Request, G53HttpResponse>
{
    public override void Configure()
    {
        Put("G53/comment/edit");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G53AuthorizationPreProcessor>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for updating artwork comment.";
            summary.Description = "This endpoint is used for updating artwork comment.";
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
                StatusCode = G53ResponseStatusCode.SUCCESS,
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
