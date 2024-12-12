using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM5
{
    public class SM5Request : IFeatureRequest<SM5Response>
    {
        /// <summary>
        ///     This value is used to mark the current
        ///     request will resolve for guest user.
        /// </summary>
        public const int GUEST_USER_ID = -1;

        public long UserId { get; set; }
        public long GroupId { get; set; }
    }
}
