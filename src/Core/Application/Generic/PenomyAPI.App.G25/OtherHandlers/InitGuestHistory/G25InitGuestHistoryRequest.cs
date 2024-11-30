using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.InitGuestHistory;

public sealed class G25InitGuestHistoryRequest
    : IFeatureRequest<G25InitGuestHistoryResponse>
{
    public static readonly G25InitGuestHistoryRequest Instance = new()
    {
    };
}
