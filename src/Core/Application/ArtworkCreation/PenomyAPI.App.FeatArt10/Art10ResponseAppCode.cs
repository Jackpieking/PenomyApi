﻿
namespace PenomyAPI.App.FeatArt10;

public enum Art10ResponseAppCode
{
    SUCCESS = 0,

    FILE_SERVICE_ERROR,

    DATABASE_ERROR,

    INVALID_FILE_EXTENSION,

    INVALID_FILE_FORMAT,

    FILE_SIZE_IS_EXCEED_THE_LIMIT,

    CHAPTER_IMAGE_LIST_IS_EMPTY,
}
