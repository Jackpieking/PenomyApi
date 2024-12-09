using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;

public class Art3RemoveDeletedItemsResponse : IFeatureResponse
{
    public Art3RemoveDeletedItemsResponseAppCode AppCode { get; set; }

    public static readonly Art3RemoveDeletedItemsResponse SUCCESS = new()
    {
        AppCode = Art3RemoveDeletedItemsResponseAppCode.SUCCESS,
    };

    public static readonly Art3RemoveDeletedItemsResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = Art3RemoveDeletedItemsResponseAppCode.CREATOR_HAS_NO_PERMISSION,
    };

    public static readonly Art3RemoveDeletedItemsResponse DATABASE_ERROR = new()
    {
        AppCode = Art3RemoveDeletedItemsResponseAppCode.DATABASE_ERROR,
    };
}
