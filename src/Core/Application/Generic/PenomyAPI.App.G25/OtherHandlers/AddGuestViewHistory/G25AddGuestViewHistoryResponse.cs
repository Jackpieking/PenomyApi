using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;

public sealed class G25AddGuestViewHistoryResponse : IFeatureResponse
{
    public G25AddGuestViewHistoryResponseAppCode AppCode { get; set; }

    public static readonly G25AddGuestViewHistoryResponse SUCCESS = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.SUCCESS,
    };

    public static readonly G25AddGuestViewHistoryResponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.GUEST_ID_NOT_FOUND,
    };

    public static readonly G25AddGuestViewHistoryResponse CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.CHAPTER_IS_NOT_FOUND,
    };

    public static readonly G25AddGuestViewHistoryResponse DATABASE_ERROR = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.DATABASE_ERROR,
    };


}
