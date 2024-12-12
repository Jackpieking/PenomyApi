using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM7
{
    public class SM7Request : IFeatureRequest<SM7Response>
    {
        public long UserId { get; set; }
        public int PageNum { get; set; }
        public int GroupNum { get; set; }
    }
}
