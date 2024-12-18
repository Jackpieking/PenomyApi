using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM11;

public class SM11Request : IFeatureRequest<SM11Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
}
