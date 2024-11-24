using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM6
{
    public class SM6Request : IFeatureRequest<SM6Response>
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }
    }
}
