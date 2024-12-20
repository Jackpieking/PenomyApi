using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt20;

public class Art20Response : IFeatureResponse
{
    public Art20ResponseAppCode AppCode { get; set; }

    public static readonly Art20Response FILE_SERVICE_ERROR = new()
    {
        AppCode = Art20ResponseAppCode.FILE_SERVICE_ERROR,
    };

    public static readonly Art20Response DATABASE_ERROR = new()
    {
        AppCode = Art20ResponseAppCode.DATABASE_ERROR,
    };

    public static readonly Art20Response SUCCESS = new()
    {
        AppCode = Art20ResponseAppCode.SUCCESS,
    };
}
