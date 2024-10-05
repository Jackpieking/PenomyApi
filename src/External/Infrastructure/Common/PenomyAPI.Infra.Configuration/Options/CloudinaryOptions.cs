using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class CloudinaryOptions : AppOptions
{
    private const string RootSectionName = "Cloudinary";

    public string ComicRootFolder { get; init; }

    public string AnimationRootFolder { get; init; }

    public string CloudinaryUrl { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.Bind(RootSectionName, this);
    }
}
