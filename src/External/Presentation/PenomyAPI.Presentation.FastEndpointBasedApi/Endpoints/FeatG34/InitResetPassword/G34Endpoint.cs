using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpRequestManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34;

public sealed class G34Endpoint : Endpoint<G34HttpRequest, G34HttpResponse>
{
    private readonly string _registerMailTemplatePath;
    private readonly ForgotPasswordMailSendingOption _option;

    public G34Endpoint(
        IWebHostEnvironment webHostEnvironment,
        ForgotPasswordMailSendingOption option
    )
    {
        _option = option;

        var stringHandler = new DefaultInterpolatedStringHandler();

        stringHandler.AppendFormatted(webHostEnvironment.WebRootPath);
        stringHandler.AppendLiteral(_option.MailTemplateRelativePath);

        _registerMailTemplatePath = stringHandler.ToStringAndClear();
    }

    public override void Configure()
    {
        Post("g34/forgot-password");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G34ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for init forgot password feature";
            summary.Description = "This endpoint is used for init forgot password purpose.";
            summary.ExampleRequest = new() { Email = "string", };
            summary.Response<G34HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"G34.{G34ResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G34HttpResponse> ExecuteAsync(
        G34HttpRequest req,
        CancellationToken ct
    )
    {
        var appRequest = new G34Request
        {
            CurrentResetPasswordLink = _option.VerifyResetPasswordLink,
            Email = req.Email,
            MailTemplate = await ReadMailTemplateAsync(_registerMailTemplatePath, ct),
        };

        // Get app feature response.
        var appResponse = await FeatureExtensions.ExecuteAsync<G34Request, G34Response>(
            appRequest,
            ct
        );

        // Convert to http response.
        var httpResponse = G34HttpResponseMapper
            .Resolve(statusCode: appResponse.StatusCode)
            .Invoke(appRequest, appResponse);

        // Send http response to client.
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    private static async Task<string> ReadMailTemplateAsync(
        string templatePath,
        CancellationToken ct
    )
    {
        var strBuilder = new StringBuilder();

        using var reader = new StreamReader(templatePath);

        string line;

        while (!Equals(line = await reader.ReadLineAsync(ct), null))
        {
            strBuilder.AppendLine(line);
        }

        return strBuilder.ToString();
    }
}
