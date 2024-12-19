using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1;

public class Admin1HttpResponse
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
        public IEnumerable<CategoryDto> Categories { get; init; } = [];

        public int TotalCategories { get; init; }

        public class CategoryDto
        {
            public string Id { get; init; }

            public string Name { get; init; }

            public DateTime CreatedAt { get; init; }

            public string CreatedBy { get; init; }
        }
    }

    public class ErrorDto
    {
        public string PropertyName { get; init; }

        public string ErrorMessage { get; init; }
    }
}
