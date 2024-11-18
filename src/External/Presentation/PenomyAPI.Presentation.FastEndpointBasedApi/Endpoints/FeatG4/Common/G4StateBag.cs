namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.Common
{
    public class G4StateBag
    {
        public bool IsAuthenticated { get; set; }

        public long UserId { get; set; }

        /// <summary>
        ///     Set the state bag for guest user.
        /// </summary>
        public void AsGuestUser()
        {
            IsAuthenticated = false;
        }

        public void AuthenticateWithUserId(long userId)
        {
            IsAuthenticated = true;
            UserId = userId;
        }
    }
}
