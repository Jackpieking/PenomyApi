using System;
using System.Collections.Generic;
using PenomyAPI.App.Common.Serializer;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.HttpResponse;

[ProtoContract]
public class G4HttpResponse : IAppProtobufObject
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
    public IEnumerable<G4ResponseItemDto> Body { get; set; }

    [ProtoMember(5)]
    public IEnumerable<string> Errors { get; set; }
}
