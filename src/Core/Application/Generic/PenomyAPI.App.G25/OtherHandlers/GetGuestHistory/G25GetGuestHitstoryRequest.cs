using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;

public sealed class G25GetGuestHitstoryRequest
    : IFeatureRequest<G25GetGuestHistoryResponse>
{
    public long GuestId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
