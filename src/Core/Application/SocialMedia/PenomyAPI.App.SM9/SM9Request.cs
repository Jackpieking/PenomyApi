using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM9
{
    public class SM9Request : IFeatureRequest<SM9Response>
    {
        public string UserId { get; set; }

        public int MaxRecord { get; set; }
    }
}
