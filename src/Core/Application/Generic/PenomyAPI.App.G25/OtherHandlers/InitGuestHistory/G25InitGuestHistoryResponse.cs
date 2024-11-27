using PenomyAPI.App.Common;
using System;

namespace PenomyAPI.App.G25.OtherHandlers.InitGuestHistory;

public sealed class G25InitGuestHistoryResponse : IFeatureResponse
{
    public G25InitGuestHistoryResponseAppCode AppCode { get; set; }

    public long GuestId { get; set; }

    public DateTime LastActiveAt { get; set; }

    public static readonly G25InitGuestHistoryResponse DATABASE_ERROR = new()
    {
        AppCode = G25InitGuestHistoryResponseAppCode.DATABASE_ERROR,
    };

    public static G25InitGuestHistoryResponse SUCCESS(long guestId) => new()
    {
        AppCode = G25InitGuestHistoryResponseAppCode.SUCCESS,
        LastActiveAt = DateTime.UtcNow,
        GuestId = guestId
    };
}
