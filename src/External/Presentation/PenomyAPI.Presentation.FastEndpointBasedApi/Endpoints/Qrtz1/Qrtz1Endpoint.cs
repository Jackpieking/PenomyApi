using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Validation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1;

public sealed class Qrtz1Endpoint : Endpoint<Qrtz1HttpRequest, Qrtz1HttpResponse>
{
    public override void Configure()
    {
        Get("/qrtz1/{AdminApiKey}");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Qrtz1ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for get all backgroundjob from quartz feature";
            summary.Description =
                "This endpoint is used for get all backgroundjob from quartz purpose.";
            summary.ExampleRequest = new Qrtz1HttpRequest() { AdminApiKey = "string" };
            summary.Response<Qrtz1HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { }
            );
        });
    }

    public override async Task<Qrtz1HttpResponse> ExecuteAsync(
        Qrtz1HttpRequest req,
        CancellationToken ct
    )
    {
        await SendAsync(null, StatusCodes.Status200OK, ct);

        return null;
    }
}
