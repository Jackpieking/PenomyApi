using PenomyAPI.App.Common;

namespace PenomyAPI.App.G13;

public class G13Request : IFeatureRequest<G13Response>
{
    public bool Empty { get; set; } = true;
}
