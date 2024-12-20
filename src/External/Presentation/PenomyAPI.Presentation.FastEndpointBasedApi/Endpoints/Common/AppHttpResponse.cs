using System;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

[ProtoContract]
public class AppHttpResponse<TBody>
{
    [ProtoMember(1)]
    public int HttpCode { get; set; }

    [ProtoMember(2)]
    public string AppCode { get; set; }

    [ProtoMember(3)]
    public DateTime ResponseTime { get; set; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            dateTime: DateTime.UtcNow,
            destinationTimeZone: TimeZoneInfo.FindSystemTimeZoneById(id: "SE Asia Standard Time")
        );

    [ProtoMember(4)]
    public TBody Body { get; set; }

    [ProtoMember(5)]
    public object Errors { get; set; }
}
