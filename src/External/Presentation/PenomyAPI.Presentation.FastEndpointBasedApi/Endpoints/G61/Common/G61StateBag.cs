using PenomyAPI.App.FeatG5;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Common;

public sealed class G61StateBag
{
    /// <summary>
    ///     Flag to indicate if the user has signed in
    ///     and authenticated or not to return the data properly. 
    /// </summary>
    public bool IsAuthenticated { get; set; }

    public long UserId { get; set; }

    /// <summary>
    ///     Set the state bag for guest user.
    /// </summary>
    public void AsGuestUser()
    {
        IsAuthenticated = false;
        UserId = G5Request.GUEST_USER_ID;
    }

    public void AuthenticateWithUserId(long userId)
    {
        IsAuthenticated = true;
        UserId = userId;
    }
}
