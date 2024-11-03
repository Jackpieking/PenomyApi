using PenomyAPI.App.Common;

namespace PenomyAPI.App.G62
{
    public class G62Request : IFeatureRequest<G62Response>
    {
        public long UserId { get; set; }

        public long CreatorId { get; set; }
    }
}
