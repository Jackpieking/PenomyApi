using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2;

public class Admin2HttpResponse
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
        public CategoryDto CategoryDetail { get; init; }

        public class CategoryDto
        {
            public string Id { get; init; }

            public string Name { get; init; }

            public string Description { get; init; }

            public DateTime UpdatedAt { get; init; }

            public string UpdatedBy { get; init; }
        }
    }

    public class ErrorDto
    {
        public string PropertyName { get; init; }

        public string ErrorMessage { get; init; }
    }
}
