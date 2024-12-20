using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt10;

public sealed class Art10Response : IFeatureResponse
{
    public Art10ResponseAppCode AppCode { get; set; }

    public static readonly Art10Response FILE_SERVICE_ERROR = new()
    {
        AppCode = Art10ResponseAppCode.FILE_SERVICE_ERROR,
    };

    public static readonly Art10Response DATABASE_ERROR = new()
    {
        AppCode = Art10ResponseAppCode.DATABASE_ERROR,
    };

    public static readonly Art10Response SUCCESS = new()
    {
        AppCode = Art10ResponseAppCode.SUCCESS,
    };
}
