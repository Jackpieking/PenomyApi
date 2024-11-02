using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt12;

public sealed class Art12Response : IFeatureResponse
{
    public Art12ResponseAppCode AppCode { get; set; }

    public static readonly Art12Response SUCCESS = new()
    {
        AppCode = Art12ResponseAppCode.SUCCESS,
    };

    public static readonly Art12Response DATABASE_ERROR = new()
    {
        AppCode = Art12ResponseAppCode.DATABASE_ERROR,
    };

    public static readonly Art12Response INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS = new()
    {
        AppCode = Art12ResponseAppCode.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS,
    };

    public static readonly Art12Response INVALID_FILE_FORMAT = new()
    {
        AppCode = Art12ResponseAppCode.INVALID_FILE_FORMAT,
    };

    public static readonly Art12Response FILE_SERVICE_ERROR = new()
    {
        AppCode = Art12ResponseAppCode.FILE_SERVICE_ERROR,
    };

    public static readonly Art12Response CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = Art12ResponseAppCode.CHAPTER_IS_NOT_FOUND,
    };

    public static readonly Art12Response CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art12ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
    };

    public static readonly Art12Response NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        AppCode = Art12ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
    };

    public static readonly Art12Response INVALID_PUBLISH_STATUS = new()
    {
        AppCode = Art12ResponseAppCode.INVALID_PUBLISH_STATUS,
    };
}
