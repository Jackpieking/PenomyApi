﻿using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;

public class G25RemoveUserHistoryItemRequest
    : IFeatureRequest<G25RemoveUserHistoryItemReponse>
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }
}
