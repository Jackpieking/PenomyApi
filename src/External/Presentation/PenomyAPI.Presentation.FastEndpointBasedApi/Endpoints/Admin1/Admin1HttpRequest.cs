namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1;

public class Admin1HttpRequest
{
    public long CurrentCategoryId { get; init; }

    public int NumberOfCategoriesToTake { get; init; }

    public string AdminApiKey { get; init; }
}
