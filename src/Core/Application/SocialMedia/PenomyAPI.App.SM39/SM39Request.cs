using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM39;

public class SM39Request : IFeatureRequest<SM39Response>
{
    public string GroupId { get; set; }
}
