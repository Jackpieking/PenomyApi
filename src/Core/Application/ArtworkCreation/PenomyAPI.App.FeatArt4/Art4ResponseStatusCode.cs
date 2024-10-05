namespace PenomyAPI.App.FeatArt4;

public enum Art4ResponseStatusCode
{
    SUCCESS = 1,

    DATABASE_ERROR = 2,

    INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES = 3,

    INVALID_FILE_EXTENSION = 4,

    FILE_SERVICE_ERROR = 5,
}
