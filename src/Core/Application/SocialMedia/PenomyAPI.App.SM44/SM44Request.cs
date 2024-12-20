using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM44;

public class SM44Request : IFeatureRequest<SM44Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
}
