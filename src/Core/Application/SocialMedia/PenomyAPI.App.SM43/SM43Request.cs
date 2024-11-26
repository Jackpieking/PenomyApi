using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM43;

public class SM43Request : IFeatureRequest<SM43Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public long MemberId { get; set; }
}
