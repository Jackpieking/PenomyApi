﻿namespace PenomyAPI.App.FeatArt22;

public enum Art22ResponseAppCode
{
    SUCCESS = 1,

    FILE_SERVICE_ERROR,

    INVALID_FILE_FORMAT,

    FILE_SIZE_IS_EXCEED_THE_LIMIT,

    CHAPTER_IS_TEMPORARILY_REMOVED,

    NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,

    INVALID_PUBLISH_STATUS,

    DATABASE_ERROR,
}
