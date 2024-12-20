using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM50;

public class SM50Request : IFeatureRequest<SM50Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
}
