using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM42;

public class SM42Request : IFeatureRequest<SM42Response>
{
    // public long UserId { get; set; }
    public long GroupId { get; set; }
}
