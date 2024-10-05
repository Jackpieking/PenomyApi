using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG7;

public class G7Request : IFeatureRequest<G7Response>
{
    public long Id { get; set; }
    public int StartPage { get; set; }
    public int PageSize { get; set; }
}
