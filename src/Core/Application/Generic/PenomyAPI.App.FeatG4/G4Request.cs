using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG4;

public class G4Request : IFeatureRequest<G4Response>
{
    /// <summary>
    ///     Flag that indicates to serve for already signed in user.
    /// </summary>
    public bool ForSignedInUser { get; set; }

    public long GuestId { get; set; }

    public long UserId { get; set; }
}
