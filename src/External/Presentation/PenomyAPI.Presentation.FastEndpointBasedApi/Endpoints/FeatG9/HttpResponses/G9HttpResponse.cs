using System;
using System.Collections.Generic;
using System.Linq;
using PenomyAPI.App.FeatG9;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;

[ProtoContract]
public sealed class G9HttpResponse
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
    public G9ResponseDto Body { get; set; }

    [ProtoMember(5)]
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public static string GetAppCode(G9ResponseAppCode appCode)
    {
        return $"G9.{appCode}.{(int)appCode}";
    }
}
