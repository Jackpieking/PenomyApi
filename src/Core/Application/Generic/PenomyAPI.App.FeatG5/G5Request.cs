using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5;

public class G5Request : IFeatureRequest<G5Response>
{
    /// <summary>
    ///     This value is used to mark the current
    ///     request will resolve for guest user.
    /// </summary>
    public const int GUEST_USER_ID = -1;

    public long ComicId { get; set; }

    public long UserId { get; set; }

    public long GuestId { get; set; }

    public bool ForSignedInUser { get; set; }
}
