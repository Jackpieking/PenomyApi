using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG33;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33;

internal sealed class G33Endpoint : Endpoint<EmptyRequest, G33HttpResponse>
{
    public override void Configure()
    {
        Post("g33/logout");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G33AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for logout feature [Test ci cd]";
            summary.Description = "This endpoint is used for logout purpose.";
            summary.ExampleRequest = new() { };
            summary.Response<G33HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"G33.{G33ResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G33HttpResponse> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var stateBag = ProcessorState<G33StateBag>();

        // Get app feature response.
        var appResponse = await FeatureExtensions.ExecuteAsync<G33Request, G33Response>(
            stateBag.AppRequest,
            ct
        );

        // Convert to http response.
        var httpResponse = G33HttpResponseMapper
            .Resolve(appResponse.StatusCode)
            .Invoke(stateBag.AppRequest, appResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);

        return httpResponse;
    }
}
