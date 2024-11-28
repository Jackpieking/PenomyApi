using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Middleware.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Middleware.Validation;
using Quartz;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2;

public sealed class Qrtz2Endpoint : Endpoint<Qrtz2HttpRequest, Qrtz2HttpResponse>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public Qrtz2Endpoint(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public override void Configure()
    {
        Get("/Qrtz2/{JobKey}");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Qrtz2ValidationPreProcessor>();
        PreProcessor<Qrtz2AuthorizationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });
        Summary(summary =>
        {
            summary.Summary =
                "Endpoint for get background job details by job key from quartz feature";
            summary.Description =
                "This endpoint is used for get background job details by job key from quartz purpose.";
            summary.ExampleRequest = new Qrtz2HttpRequest() { AdminApiKey = "string" };
            summary.Response<Qrtz2HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { }
            );
        });
    }

    public override async Task<Qrtz2HttpResponse> ExecuteAsync(
        Qrtz2HttpRequest req,
        CancellationToken ct
    )
    {
        // Get current scheduler.
        var scheduler = await _schedulerFactory.GetScheduler(ct);

        // Get all jobs info from current scheduler.
        var job = await GetJobInfoFromSchedulerByJobKeyAsync(scheduler, req.JobKey, ct);

        var httpResponse = new Qrtz2HttpResponse { Body = job };

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private async Task<Qrtz2HttpResponse.JobDetailDto> GetJobInfoFromSchedulerByJobKeyAsync(
        IScheduler scheduler,
        string jobKeyName,
        CancellationToken ct
    )
    {
        // Init the job key with input job key name
        var jobKey = new JobKey(jobKeyName);

        // Then, Get job detail by job key.
        var foundJobKey = await scheduler.GetJobDetail(jobKey, ct);

        // Then assign result to a new job detail dto
        var jobDetailDto = new Qrtz2HttpResponse.JobDetailDto
        {
            JobKey = foundJobKey.Key.Name,
            JobDescription = foundJobKey.Description,
            JobType = foundJobKey.JobType.Name,
            Triggers = new List<Qrtz2HttpResponse.TriggerDetailDto>()
        };

        // Then get all triggers of that job
        var triggers = await scheduler.GetTriggersOfJob(jobKey, ct);

        foreach (var trigger in triggers)
        {
            // Then assign each trigger info to a new trigger detail dto
            var triggerDetailDto = new Qrtz2HttpResponse.TriggerDetailDto
            {
                TriggerKey = trigger.Key.Name,
                TriggerState = (await scheduler.GetTriggerState(trigger.Key, ct)).ToString(),
                PreviousFireTime = trigger.GetPreviousFireTimeUtc()?.LocalDateTime,
                NextFireTime = trigger.GetNextFireTimeUtc()?.LocalDateTime
            };

            // Then add trigger detail to the trigger
            // list in job detail
            jobDetailDto.Triggers.Add(triggerDetailDto);
        }

        return jobDetailDto;
    }
}
