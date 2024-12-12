using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;

public class Art3RestoreDeletedItemResponse : IFeatureResponse
{
    public Art3RestoreDeletedItemsResponseAppCode AppCode { get; set; }

    public static readonly Art3RestoreDeletedItemResponse SUCCESS = new()
    {
        AppCode = Art3RestoreDeletedItemsResponseAppCode.SUCCESS,
    };

    public static readonly Art3RestoreDeletedItemResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = Art3RestoreDeletedItemsResponseAppCode.CREATOR_HAS_NO_PERMISSION,
    };

    public static readonly Art3RestoreDeletedItemResponse DATABASE_ERROR = new()
    {
        AppCode = Art3RestoreDeletedItemsResponseAppCode.DATABASE_ERROR,
    };
}
