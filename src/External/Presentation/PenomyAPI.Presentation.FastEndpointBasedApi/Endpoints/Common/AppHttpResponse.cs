using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

public abstract class AppHttpResponse<TBody>
{
    public int HttpCode { get; set; }

    public string AppCode { get; set; }

    public DateTime ResponseTime { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(
        dateTime: DateTime.UtcNow,
        destinationTimeZone: TimeZoneInfo.FindSystemTimeZoneById(id: "SE Asia Standard Time")
    );

    public TBody Body { get; set; }

    public object Errors { get; set; }
}
