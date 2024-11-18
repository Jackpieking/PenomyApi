using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM1
{
    public class SM1Request : IFeatureRequest<SM1Response>
    {
        public long UserId { get; set; }
    }
}
