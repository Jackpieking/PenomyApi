using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;

public class G25RemoveGuestHistoryItemRequest
    : IFeatureRequest<G25RemoveGuestHistoryItemReponse>
{
    public long GuestId { get; set; }

    public long ArtworkId { get; set; }
}
