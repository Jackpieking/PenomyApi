using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken;

public class G1VerifyRegistrationTokenEndpoint
    : Endpoint<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenHttpResponse>
{
    public override void Configure()
    {
        Get("g1/register/verify/{registrationToken}");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G1VerifyRegistrationTokenValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for verifying registration token feature";
            summary.Description = "This endpoint is used for verifying registration token purpose.";
            summary.ExampleRequest = new() { RegistrationToken = "string", };
            summary.Response<G1VerifyRegistrationTokenHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode =
                        $"G1VerifyRegistrationToken.{G1VerifyRegistrationTokenResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G1VerifyRegistrationTokenHttpResponse> ExecuteAsync(
        G1VerifyRegistrationTokenRequest req,
        CancellationToken ct
    )
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<
            G1VerifyRegistrationTokenRequest,
            G1VerifyRegistrationTokenResponse
        >(req, ct);

        // Convert to http response.
        var httpResponse = G1VerifyRegistrationTokenHttpResponseMapper
            .Resolve(statusCode: featureResponse.StatusCode)
            .Invoke(req, featureResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
