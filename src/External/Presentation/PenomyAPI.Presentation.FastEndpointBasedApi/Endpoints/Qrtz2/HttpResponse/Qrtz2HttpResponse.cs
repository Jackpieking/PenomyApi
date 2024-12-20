using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpResponse;

public sealed class Qrtz2HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public object Body { get; init; } = new();

    public object Errors { get; init; } = new();

    public sealed class JobDetailDto
    {
        public string JobKey { get; init; }
        public string JobDescription { get; init; }
        public string JobType { get; init; }
        public List<TriggerDetailDto> Triggers { get; init; }
    }

    public sealed class TriggerDetailDto
    {
        public string TriggerKey { get; init; }
        public string TriggerState { get; init; }
        public DateTime? PreviousFireTime { get; init; }
        public DateTime? NextFireTime { get; init; }
    }
}
