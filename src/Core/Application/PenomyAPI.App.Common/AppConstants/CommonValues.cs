using System;

namespace PenomyAPI.App.Common.AppConstants;

public static class CommonValues
{
    public static class DateTimes
    {
        /// <summary>
        ///     This min value will be used when creating a new entity that
        ///     has TemporarilyRemovedAt property is not needed to used immediately.
        /// </summary>
        /// <remarks>
        ///     The below value is referenced from: <see href="https://penntoday.upenn.edu/news/worlds-first-general-purpose-computer-turns-75"/>
        /// </remarks>
        public static readonly DateTime MinUtc = new(1946, 2, 14, 0, 0, 0, DateTimeKind.Utc);
    }

    public static class Claims
    {
        public const string AppUserEmailClaim = "app-user-email";

        public static class TokenPurpose
        {
            public const string Type = "purpose";

            public static class Values
            {
                public const string ResetPassword = "reset-password";

                public const string VerifyEmail = "verify-email";

                public const string AppUserAccess = "app-user-access";
            }
        }

        public const string TokenIdClaim = "jti";

        public const string UserIdClaim = "sub";
    }

    public static class Others
    {
        public const string DefaultUserAvaterUrl =
            "https://res.cloudinary.com/dsjsmbdpw/image/upload/v1729831342/penomy_assets/default_avatar_url.webp";
    }
}
