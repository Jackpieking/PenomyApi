using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt8;

public sealed class Art8Response : IFeatureResponse
{
    public Art8ResponseAppCode AppCode { get; set; }

    public static readonly Art8Response SUCCESS = new()
    {
        AppCode = Art8ResponseAppCode.SUCCESS
    };

    public static readonly Art8Response NOT_FOUND = new()
    {
        AppCode = Art8ResponseAppCode.ARTWORK_ID_NOT_FOUND
    };

    public static readonly Art8Response ALREADY_REMOVED = new()
    {
        AppCode = Art8ResponseAppCode.ARTWORK_IS_ALREADY_REMOVED
    };
    
    public static readonly Art8Response DATABASE_ERROR = new()
    {
        AppCode = Art8ResponseAppCode.DATABASE_ERROR
    };
}
