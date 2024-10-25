using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration;

public sealed class G1CompleteRegistrationEndpoint
    : Endpoint<G1CompleteRegistrationRequest, G1CompleteRegistrationHttpResponse>
{
    public override void Configure()
    {
        Post("g1/register/complete");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G1CompleteRegistrationValidationPreProcessor>();
        PreProcessor<G1CompleteRegistrationCachingPreProcessor>();
        PostProcessor<G1CompleteRegistrationCachingPostProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for completing registration token feature";
            summary.Description =
                "This endpoint is used for completing registration token purpose.";
            summary.ExampleRequest = new()
            {
                PreRegistrationToken = "string",
                ConfirmedNickName = "string",
                Password = "string"
            };
            summary.Response<G1CompleteRegistrationHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G1CompleteRegistrationHttpResponse> ExecuteAsync(
        G1CompleteRegistrationRequest req,
        CancellationToken ct
    )
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<
            G1CompleteRegistrationRequest,
            G1CompleteRegistrationResponse
        >(req, ct);

        // Convert to http response.
        var httpResponse = G1CompleteRegistrationHttpResponseMapper
            .Resolve(statusCode: featResponse.StatusCode)
            .Invoke(req, featResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
