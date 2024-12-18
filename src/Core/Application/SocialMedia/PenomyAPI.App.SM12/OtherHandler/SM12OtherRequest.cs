using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM12.OtherHandler;

public class SM12OtherRequest : IFeatureRequest<SM12OtherResponse>
{
    public long UserId { get; set; }
}
