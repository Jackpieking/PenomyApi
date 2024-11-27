using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;

public sealed class G25GetGuestTrackingResponse : IFeatureResponse
{
    public G25GetGuestTrackingResponseAppCode AppCode { get; set; }

    public string GuestId { get; set; }

    public DateTime LastActiveAt { get; set; }

    public static G25GetGuestTrackingResponse SUCCESS(GuestTracking guestTracking)
    {
        return new()
        {
            AppCode = G25GetGuestTrackingResponseAppCode.SUCCESS,
            GuestId = guestTracking.GuestId.ToString(),
            LastActiveAt = guestTracking.LastActiveAt,
        };
    }

    public static readonly G25GetGuestTrackingResponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25GetGuestTrackingResponseAppCode.GUEST_ID_NOT_FOUND,
    };
}
