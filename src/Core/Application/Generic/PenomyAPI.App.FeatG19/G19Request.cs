using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG19;

public class G19Request : IFeatureRequest<G19Response>
{
    public long Id { get; set; }
    public int StartPage { get; set; }
    public int PageSize { get; set; }
}
