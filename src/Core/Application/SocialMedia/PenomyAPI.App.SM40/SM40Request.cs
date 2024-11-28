using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM40;

public class SM40Request : IFeatureRequest<SM40Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public long MemberId { get; set; }
}
