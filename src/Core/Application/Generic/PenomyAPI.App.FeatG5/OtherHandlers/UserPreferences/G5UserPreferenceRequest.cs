using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;

public class G5UserPreferenceRequest : IFeatureRequest<G5UserPreferenceResponse>
{
    private const int UNDEFINED_USER_ID = -1;

    public long UserId { get; set; }

    public long GuestId { get; set; }

    public long ArtworkId { get; set; }

    public bool IsGuest { get; set; }

    public static G5UserPreferenceRequest ForGuest(
        long guestId,
        long artworkId)
        => new()
        {
            IsGuest = true,
            GuestId = guestId,
            ArtworkId = artworkId,
            UserId = UNDEFINED_USER_ID,
        };

    public static G5UserPreferenceRequest ForUser(
        long userId,
        long artworkId)
        => new()
        {
            IsGuest = false,
            UserId = userId,
            ArtworkId = artworkId,
            GuestId = UNDEFINED_USER_ID,
        };
}
