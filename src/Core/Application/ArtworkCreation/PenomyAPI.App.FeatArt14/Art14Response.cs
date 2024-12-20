using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt14;

public sealed class Art14Response : IFeatureResponse
{
    public Art14ResponseAppCode AppCode { get; private set; }

    public static readonly Art14Response SUCCESS = new()
    {
        AppCode = Art14ResponseAppCode.SUCCESS,
    };

    public static readonly Art14Response CHAPTER_ID_NOT_FOUND = new()
    {
        AppCode = Art14ResponseAppCode.CHAPTER_ID_NOT_FOUND,
    };

    public static readonly Art14Response CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = Art14ResponseAppCode.CREATOR_HAS_NO_PERMISSION,
    };

    public static readonly Art14Response DATABASE_ERROR = new()
    {
        AppCode = Art14ResponseAppCode.DATABASE_ERROR,
    };
}
