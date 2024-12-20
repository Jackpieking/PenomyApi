using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4;

public class Admin4HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public BodyDto Body { get; init; }

    public IEnumerable<ErrorDto> Errors { get; init; } = [];

    public class BodyDto
    {
        public string AccessToken { get; init; }

        public UserInfoDto UserInfo { get; init; }

        public class UserInfoDto
        {
            public string Email { get; init; }
        }
    }

    public class ErrorDto
    {
        public string PropertyName { get; init; }

        public string ErrorMessage { get; init; }
    }
}
