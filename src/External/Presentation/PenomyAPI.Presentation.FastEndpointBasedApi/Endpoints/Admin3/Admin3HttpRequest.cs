namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3;

public class Admin3HttpRequest
{
    public string AdminApiKey { get; init; }

    public CategoryDto Category { get; init; }

    public class CategoryDto
    {
        public long Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }
    }
}
