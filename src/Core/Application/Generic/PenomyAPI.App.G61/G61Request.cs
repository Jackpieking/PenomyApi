using PenomyAPI.App.Common;

namespace PenomyAPI.App.G61
{
    public class G61Request : IFeatureRequest<G61Response>
    {
        public long UserId { get; set; }
        public long CreatorId { get; set; }
    }
}
