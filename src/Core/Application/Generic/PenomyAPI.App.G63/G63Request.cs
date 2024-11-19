using PenomyAPI.App.Common;

namespace PenomyAPI.App.G63
{
    public class G63Request : IFeatureRequest<G63Response>
    {
        public long UserId { get; set; }
        public int PageNum { get; set; }
        public int CreatorNum { get; set; }
    }
}
