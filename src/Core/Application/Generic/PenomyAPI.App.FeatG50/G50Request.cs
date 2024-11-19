using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG50;

public class G50Request : IFeatureRequest<G50Response>
{
    private long UserId { get; set; }
    public long ArtworkId { get; set; }

    public long GetUserId()
    {
        return UserId;
    }

    public void SetUserId(long userId)
    {
        UserId = userId;
    }
}
