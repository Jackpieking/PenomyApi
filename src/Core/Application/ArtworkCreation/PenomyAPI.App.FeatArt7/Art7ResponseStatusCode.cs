namespace PenomyAPI.App.FeatArt7;

public enum Art7ResponseStatusCode
{
    SUCCESS = 0,

    COMIC_ID_NOT_FOUND = 1,

    DATABASE_ERROR = 2,

    FILE_SERVICE_ERROR = 3,

    INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES = 4,

    INVALID_FILE_EXTENSION = 5,
}
