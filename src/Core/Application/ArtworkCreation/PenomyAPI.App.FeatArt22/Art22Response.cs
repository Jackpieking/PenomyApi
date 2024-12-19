using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt22;

public class Art22Response : IFeatureResponse
{
    public Art22ResponseAppCode AppCode { get; set; }

    public static readonly Art22Response SUCCESS = new()
    {
        AppCode = Art22ResponseAppCode.SUCCESS,
    };

    public static readonly Art22Response DATABASE_ERROR = new()
    {
        AppCode = Art22ResponseAppCode.DATABASE_ERROR,
    };

    public static readonly Art22Response FILE_SERVICE_ERROR = new()
    {
        AppCode = Art22ResponseAppCode.FILE_SERVICE_ERROR,
    };

    public static readonly Art22Response CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = Art22ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
    };

    public static readonly Art22Response NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        AppCode = Art22ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
    };

    public static readonly Art22Response INVALID_PUBLISH_STATUS = new()
    {
        AppCode = Art22ResponseAppCode.INVALID_PUBLISH_STATUS,
    };
}
