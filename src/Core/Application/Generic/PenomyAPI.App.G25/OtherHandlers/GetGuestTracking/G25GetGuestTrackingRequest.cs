﻿using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;

public sealed class G25GetGuestTrackingRequest : IFeatureRequest<G25GetGuestTrackingResponse>
{
    public long GuestId { get; set; }
}
