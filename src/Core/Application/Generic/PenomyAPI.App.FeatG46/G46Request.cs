using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG46;

public class G46Request : IFeatureRequest<G46Response>
{
    public long UserId { get; set; }
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
