using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG8
{
    public class G8Request : IFeatureRequest<G8Response>
    {
        public long Id { get; set; }
        public int StartPage { get; set; }
        public int PageSize { get; set; }
    }
}
