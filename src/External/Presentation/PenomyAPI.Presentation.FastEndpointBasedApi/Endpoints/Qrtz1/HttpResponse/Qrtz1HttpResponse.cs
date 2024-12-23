using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpResponse;

public sealed class Qrtz1HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public BodyDto Body { get; init; } = new();

    public object Errors { get; init; } = new();

    public sealed class BodyDto
    {
        public IEnumerable<string> JobKeys { get; init; }
    }
}
