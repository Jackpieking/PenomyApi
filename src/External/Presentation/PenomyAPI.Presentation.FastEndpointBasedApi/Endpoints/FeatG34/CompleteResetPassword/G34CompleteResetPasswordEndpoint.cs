using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.Middleware.Validation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword;

public sealed class G34CompleteResetPasswordEndpoint
    : Endpoint<G34CompleteResetPasswordRequest, G34CompleteResetPasswordHttpResponse>
{
    public override void Configure()
    {
        Post("g34/forgot-password/complete");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G34CompleteResetPasswordValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for complete reset password process feature";
            summary.Description =
                "This endpoint is used for complete reset password process purpose.";
            summary.ExampleRequest = new G34CompleteResetPasswordRequest()
            {
                Email = "string",
                NewPassword = "string",
                ResetPasswordTokenId = "string"
            };
            summary.Response<G34CompleteResetPasswordHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G34CompleteResetPasswordHttpResponse> ExecuteAsync(
        G34CompleteResetPasswordRequest req,
        CancellationToken ct
    )
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<
            G34CompleteResetPasswordRequest,
            G34CompleteResetPasswordResponse
        >(req, ct);

        // Convert to http response.
        var httpResponse = G34CompleteResetPasswordHttpResponseMapper
            .Resolve(statusCode: featureResponse.StatusCode)
            .Invoke(req, featureResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
