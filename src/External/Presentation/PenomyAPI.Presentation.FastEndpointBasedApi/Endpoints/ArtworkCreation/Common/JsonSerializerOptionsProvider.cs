using System.Text.Json;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common;

public static class JsonSerializerOptionsProvider
{
    private static object _lock = new();
    /// <summary>
    ///     A singleton instance of json serializer
    ///     options to reuse for next time.
    /// </summary>
    private static JsonSerializerOptions _jsonSerializerOptions;

    /// <summary>
    ///     Get the completed setup <see cref="JsonSerializerOptions"/> instance.
    /// </summary>
    public static JsonSerializerOptions Get()
    {
        lock (_lock)
        {
            if (Equals(_jsonSerializerOptions, null))
            {
                _jsonSerializerOptions = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    PropertyNameCaseInsensitive = true
                };
            }
        }

        return _jsonSerializerOptions;
    }
}
