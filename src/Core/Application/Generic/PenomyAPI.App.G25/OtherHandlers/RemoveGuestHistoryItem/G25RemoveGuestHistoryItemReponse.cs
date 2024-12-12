using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;

public class G25RemoveGuestHistoryItemReponse : IFeatureResponse
{
    public G25RemoveGuestHistoryItemResponseAppCode AppCode { get; set; }

    public static readonly G25RemoveGuestHistoryItemReponse SUCCESS = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.SUCCESS,
    };

    public static readonly G25RemoveGuestHistoryItemReponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.GUEST_ID_NOT_FOUND,
    };

    public static readonly G25RemoveGuestHistoryItemReponse DATABASE_ERROR = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.DATABASE_ERROR,
    };
}
