namespace PenomyAPI.App.FeatArt12;

public enum Art12ResponseAppCode
{
    SUCCESS = 0,

    /// <summary>
    ///     This app is used when found more than 1 chapter media
    ///     item that have similar upload order.
    /// </summary>
    INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS = 1,

    FILE_SERVICE_ERROR = 2,

    INVALID_FILE_FORMAT = 3,

    FILE_SIZE_IS_EXCEED_THE_LIMIT = 4,

    CHAPTER_IS_NOT_FOUND = 5,

    CHAPTER_IS_TEMPORARILY_REMOVED = 6,

    NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = 7,

    INVALID_PUBLISH_STATUS = 8,

    DATABASE_ERROR = 9,
}
