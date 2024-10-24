using System;
using System.Collections.Generic;
using PenomyAPI.App.Common.Serializer;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Caching;

[ProtoContract]
public sealed class G1HttpResponseCaching : IAppProtobufObject
{
    [ProtoMember(tag: 1)]
    public int HttpCode { get; set; }

    [ProtoMember(tag: 2)]
    public string AppCode { get; init; }

    [ProtoMember(tag: 3)]
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    [ProtoMember(tag: 4)]
    public IEnumerable<string> Body { get; init; }

    [ProtoMember(tag: 5)]
    public IEnumerable<string> Errors { get; init; }
}
