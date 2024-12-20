using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG35;

public sealed class G35Request : IFeatureRequest<G35Response>
{
    public long UserId { get; set; }

    /// <summary>
    ///     This flag to indicate the request is served for
    ///     the owner of this user profile.
    /// </summary>
    public bool IsProfileOwner { get; set; }
}
