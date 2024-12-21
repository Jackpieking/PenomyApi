using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31;

internal sealed class G31Endpoint : Endpoint<G31Request, G31HttpResponse>
{
    public override void Configure()
    {
        Post("g31/login");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G31ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for feature";
            summary.Description = "This endpoint is used for login purpose.";
            summary.ExampleRequest = new()
            {
                Email = "string",
                Password = "string",
                RememberMe = true
            };
            summary.Response<G31HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"G31.{G31ResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G31HttpResponse> ExecuteAsync(G31Request req, CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<G31Request, G31Response>(req, ct);

        // Convert to http response.
        var httpResponse = G31HttpResponseMapper
            .Resolve(statusCode: featResponse.StatusCode)
            .Invoke(req, featResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
