namespace PenomyAPI.App.SM12;

public enum SM12ResponseStatusCode
{
    SUCCESS = 1,

    DATABASE_ERROR = 2,

    FILE_SERVICE_ERROR = 3,

    INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES = 4,

    INVALID_FILE_EXTENSION = 5,

    INVALID_FILE_FORMAT = 6,

    FILE_SIZE_IS_EXCEED_THE_LIMIT = 7,
    USER_PROFILE_NOT_FOUND = 8
}