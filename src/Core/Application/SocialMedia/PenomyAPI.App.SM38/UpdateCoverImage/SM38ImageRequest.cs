using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;

namespace PenomyAPI.App.SM38.CoverImage
{
    public class SM38ImageRequest : IFeatureRequest<SM38ImageResponse>
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public AppFileInfo CoverImageFileInfo { get; set; }
    }
}
