using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;

public class G25AddGuestViewHistoryRequest : IFeatureRequest<G25AddGuestViewHistoryResponse>
{
    public long GuestId { get; set; }

    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }
}
