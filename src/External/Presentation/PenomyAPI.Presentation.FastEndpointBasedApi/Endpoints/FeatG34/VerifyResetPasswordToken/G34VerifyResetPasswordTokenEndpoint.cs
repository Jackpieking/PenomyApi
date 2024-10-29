using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken;

public sealed class G34VerifyResetPasswordTokenEndpoint
    : Endpoint<G34VerifyResetPasswordTokenRequest, G34VerifyResetPasswordTokenHttpResponse>
{
    public override void Configure()
    {
        Post("g34/forgot-password/verify");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G34VerifyResetPasswordTokenValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for verifying reset password token feature";
            summary.Description =
                "This endpoint is used for verifying reset password token purpose.";
            summary.ExampleRequest = new() { ResetPasswordToken = "string", };
            summary.Response<G34VerifyResetPasswordTokenHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode =
                        $"G34VerifyResetPasswordToken.{G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G34VerifyResetPasswordTokenHttpResponse> ExecuteAsync(
        G34VerifyResetPasswordTokenRequest req,
        CancellationToken ct
    )
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<
            G34VerifyResetPasswordTokenRequest,
            G34VerifyResetPasswordTokenResponse
        >(req, ct);

        // Convert to http response.
        var httpResponse = G34VerifyResetPasswordTokenHttpResponseMapper
            .Resolve(statusCode: featureResponse.StatusCode)
            .Invoke(req, featureResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
