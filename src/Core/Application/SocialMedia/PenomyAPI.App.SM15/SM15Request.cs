using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM15;

public class SM15Request : IFeatureRequest<SM15Response>
{
    public long UserId { get; set; }
}
