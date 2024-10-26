using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Middlewares.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration;

internal sealed class G1Endpoint : Endpoint<G1HttpRequest, G1HttpResponse>
{
    private readonly string _registerMailTemplatePath;

    public G1Endpoint(IWebHostEnvironment webHostEnvironment)
    {
        var stringHandler = new DefaultInterpolatedStringHandler();

        stringHandler.AppendFormatted(webHostEnvironment.WebRootPath);
        stringHandler.AppendLiteral("\\register_mail_template.html");

        _registerMailTemplatePath = stringHandler.ToStringAndClear();
    }

    public override void Configure()
    {
        Post("g1/register");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<G1ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for init registration feature";
            summary.Description = "This endpoint is used for init registration purpose.";
            summary.ExampleRequest = new() { Email = "string", };
            summary.Response<G1HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"G1.{G1ResponseStatusCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<G1HttpResponse> ExecuteAsync(G1HttpRequest req, CancellationToken ct)
    {
        var appRequest = new G1Request
        {
            RegisterPageLink = "http://localhost:9000/auth/confirm-register",
            Email = req.Email,
            MailTemplate = await ReadMailTemplateAsync(_registerMailTemplatePath, ct),
        };

        // Get app feature response.
        var appResponse = await FeatureExtensions.ExecuteAsync<G1Request, G1Response>(
            appRequest,
            ct
        );

        // Convert to http response.
        var httpResponse = G1HttpResponseMapper
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
