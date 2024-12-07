using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;

public class Art3RestoreAllDeletedItemsResponse : IFeatureResponse
{
    public Art3RestoreAllDeletedItemsResponseAppCode AppCode { get; set; }

    public static readonly Art3RestoreAllDeletedItemsResponse SUCCESS = new()
    {
        AppCode = Art3RestoreAllDeletedItemsResponseAppCode.SUCCESS,
    };

    public static readonly Art3RestoreAllDeletedItemsResponse NO_DELETED_ITEMS_FOUND = new()
    {
        AppCode = Art3RestoreAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND,
    };

    public static readonly Art3RestoreAllDeletedItemsResponse DATABASE_ERROR = new()
    {
        AppCode = Art3RestoreAllDeletedItemsResponseAppCode.DATABASE_ERROR,
    };
}
