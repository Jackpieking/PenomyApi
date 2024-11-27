using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpResponse;

public sealed class Qrtz1HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public object Body { get; init; } = new();

    public object Errors { get; init; }
}
