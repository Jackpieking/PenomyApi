using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG4;

public class G4Request : IFeatureRequest<G4Response>
{
    public string Category { get; set; }
}
