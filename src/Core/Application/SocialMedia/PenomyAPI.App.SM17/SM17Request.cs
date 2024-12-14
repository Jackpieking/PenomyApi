using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM17;

public class SM17Request : IFeatureRequest<SM17Response>
{
    public long UserId { get; set; }

    public long PostId { get; set; }

    public bool IsGroupPost { get; set; }
}
