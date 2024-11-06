using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class CloudinaryOptions : AppOptions
{
    public string ComicRootFolder { get; init; }

    public string AnimationRootFolder { get; init; }

    public string CloudinaryUrl { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("Cloudinary").Bind(this);
    }
}
