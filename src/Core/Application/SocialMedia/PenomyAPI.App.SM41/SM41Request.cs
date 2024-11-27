using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM41;

public class SM41Request : IFeatureRequest<SM41Response>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
}
