using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31A;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Middlewares.Validation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A;

internal sealed class G31AEndpoint : Endpoint<G31ARequest, G31AHttpResponse>
{
    public override void Configure()
    {
        Post("g31A/refresh-access-token");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G31AValidationPreProcessor>();
        PreProcessor<G31AAuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for refresh access token feature";
            summary.Description = "This endpoint is used for refresh access token purpose.";
            summary.ExampleRequest = new G31ARequest() { RefreshToken = "string" };
            summary.Response<G31AHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"G31A.{G31AResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G31AHttpResponse> ExecuteAsync(G31ARequest req, CancellationToken ct)
    {
        // Get app feature response.
        var appResponse = await FeatureExtensions.ExecuteAsync<G31ARequest, G31AResponse>(req, ct);

        // Convert to http response.
        var httpResponse = G31AHttpResponseMapper
            .Resolve(appResponse.StatusCode)
            .Invoke(req, appResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);

        return httpResponse;
    }
}
