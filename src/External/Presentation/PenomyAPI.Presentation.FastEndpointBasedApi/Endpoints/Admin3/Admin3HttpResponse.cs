using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3;

public class Admin3HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public BodyDto Body { get; init; }

    public IEnumerable<ErrorDto> Errors { get; init; } = [];

    public class BodyDto { }

    public class ErrorDto
    {
        public string PropertyName { get; init; }

        public string ErrorMessage { get; init; }
    }
}
