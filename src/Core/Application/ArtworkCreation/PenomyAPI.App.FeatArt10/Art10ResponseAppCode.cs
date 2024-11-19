
namespace PenomyAPI.App.FeatArt10;

public enum Art10ResponseAppCode
{
    SUCCESS = 0,

    FILE_SERVICE_ERROR = 1,

    DATABASE_ERROR = 2,

    INVALID_FILE_EXTENSION = 3,

    INVALID_FILE_FORMAT = 4,

    FILE_SIZE_IS_EXCEED_THE_LIMIT = 5,

    CHAPTER_IMAGE_LIST_IS_EMPTY = 6,
}
