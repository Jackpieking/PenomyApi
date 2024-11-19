using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM14;

public class SM14Request : IFeatureRequest<SM14Response>
{
    public long UserId { get; set; }
    public long PostId { get; set; }
}
