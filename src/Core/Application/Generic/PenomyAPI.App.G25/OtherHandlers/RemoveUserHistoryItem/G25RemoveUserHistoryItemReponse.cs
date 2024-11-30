using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;

public class G25RemoveUserHistoryItemReponse : IFeatureResponse
{
    public G25RemoveUserHistoryItemResponseAppCode AppCode { get; set; }

    public static readonly G25RemoveUserHistoryItemReponse SUCCESS = new()
    {
        AppCode = G25RemoveUserHistoryItemResponseAppCode.SUCCESS,
    };

    public static readonly G25RemoveUserHistoryItemReponse DATABASE_ERROR = new()
    {
        AppCode = G25RemoveUserHistoryItemResponseAppCode.DATABASE_ERROR,
    };
}
