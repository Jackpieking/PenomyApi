using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;

internal sealed class G1HttpResponse
{
    public int HttpCode { get; init; }

    public string AppCode { get; init; }

    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public object Body { get; init; } = new();

    public object Errors { get; init; }
}
