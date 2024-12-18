using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt17;

public class Art17Response : IFeatureResponse
{
    public Art17ResponseAppCode AppCode { get; set; }

    #region Static readonly instances
    public static readonly Art17Response SUCCESS =
        new() { AppCode = Art17ResponseAppCode.SUCCESS};
    
    public static readonly Art17Response CURRENT_CREATOR_IS_NOT_AUTHORIZED =
        new() { AppCode = Art17ResponseAppCode.CURRENT_CREATOR_IS_NOT_AUTHORIZED };

    public static readonly Art17Response DATABASE_ERROR =
        new() { AppCode = Art17ResponseAppCode.DATABASE_ERROR };

    public static readonly Art17Response FILE_SERVICE_ERROR =
        new() { AppCode = Art17ResponseAppCode.FILE_SERVICE_ERROR };
    #endregion
}
