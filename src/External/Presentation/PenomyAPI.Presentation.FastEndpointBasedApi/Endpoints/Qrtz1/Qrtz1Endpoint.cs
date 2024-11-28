using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Validation;
using Quartz;
using Quartz.Impl.Matchers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1;

public sealed class Qrtz1Endpoint : Endpoint<Qrtz1HttpRequest, Qrtz1HttpResponse>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public Qrtz1Endpoint(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public override void Configure()
    {
        Get("/Qrtz1");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Qrtz1ValidationPreProcessor>();
        PreProcessor<Qrtz1AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for get all background job key from quartz feature";
            summary.Description =
                "This endpoint is used for get all background job key from quartz purpose.";
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
        // Get current scheduler.
        var scheduler = await _schedulerFactory.GetScheduler(ct);

        // Get all job key names from current scheduler.
        var jobs = await GetAllJobKeyNamesFromSchedulerAsync(scheduler, ct);

        var httpResponse = new Qrtz1HttpResponse { Body = new() { JobKeys = jobs } };

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private async Task<IEnumerable<string>> GetAllJobKeyNamesFromSchedulerAsync(
        IScheduler scheduler,
        CancellationToken ct
    )
    {
        // First, Get all job key from all groups.
        var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), ct);

        // Then return a list of job key names.
        var jobKeyNames = jobKeys.Select(jobKey => jobKey.Name);

        return jobKeyNames;
    }
}
