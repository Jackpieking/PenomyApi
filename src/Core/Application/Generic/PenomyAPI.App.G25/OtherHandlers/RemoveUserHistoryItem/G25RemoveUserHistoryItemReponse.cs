using PenomyAPI.App.Common;
using PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;

public class G25RemoveUserHistoryItemReponse : IFeatureResponse
{
    public G25RemoveGuestHistoryItemResponseAppCode AppCode { get; set; }

    public static readonly G25RemoveUserHistoryItemReponse SUCCESS = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.SUCCESS,
    };

    public static readonly G25RemoveUserHistoryItemReponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.GUEST_ID_NOT_FOUND,
    };

    public static readonly G25RemoveUserHistoryItemReponse DATABASE_ERROR = new()
    {
        AppCode = G25RemoveGuestHistoryItemResponseAppCode.DATABASE_ERROR,
    };
}
