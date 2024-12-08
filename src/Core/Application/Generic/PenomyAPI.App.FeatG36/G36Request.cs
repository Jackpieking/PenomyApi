using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;

namespace PenomyAPI.App.FeatG36;

public sealed class G36Request : IFeatureRequest<G36Response>
{
    public long UserId { get; set; }

    public string NickName { get; set; }

    public string AboutMe { get; set; }

    public AppFileInfo AvatarFileInfo { get; set; }

    public bool HasUpdatedAvatar => AvatarFileInfo != null;
}
