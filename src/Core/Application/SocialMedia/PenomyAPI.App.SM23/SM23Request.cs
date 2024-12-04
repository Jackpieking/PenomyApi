using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM23;

public class SM23Request : IFeatureRequest<SM23Response>
{
    public long UserId { get; set; }

    public long PostId { get; set; }
}
