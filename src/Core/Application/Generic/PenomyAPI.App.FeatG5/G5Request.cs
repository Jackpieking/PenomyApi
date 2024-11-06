using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5;

public class G5Request : IFeatureRequest<G5Response>
{
    public long Id { get; set; }
    public long UserId { get; set; }

    public long GetUserId()
    {
        return UserId;
    }

    public void SetUserId(long userId)
    {
        UserId = userId;
    }
}
