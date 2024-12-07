using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;

public class Art3RemoveAllDeletedItemsResponse
    : IFeatureResponse
{
    public Art3RemoveAllDeletedItemsResponseAppCode AppCode { get; set; }

    public static readonly Art3RemoveAllDeletedItemsResponse SUCCESS = new()
    {
        AppCode = Art3RemoveAllDeletedItemsResponseAppCode.SUCCESS,
    };
    
    public static readonly Art3RemoveAllDeletedItemsResponse NO_DELETED_ITEMS_FOUND = new()
    {
        AppCode = Art3RemoveAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND,
    };

    public static readonly Art3RemoveAllDeletedItemsResponse DATABASE_ERROR = new()
    {
        AppCode = Art3RemoveAllDeletedItemsResponseAppCode.DATABASE_ERROR,
    };
}
