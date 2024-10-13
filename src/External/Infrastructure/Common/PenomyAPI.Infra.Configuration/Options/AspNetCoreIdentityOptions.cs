namespace PenomyAPI.Infra.Configuration.Options;

public sealed class AspNetCoreIdentityOptions
{
    public PasswordOption Password { get; } = new();

    public LockoutOption Lockout { get; } = new();

    public UserOption User { get; } = new();

    public SignInOption SignIn { get; } = new();

    public sealed class PasswordOption
    {
        public bool RequireDigit { get; init; }

        public bool RequireLowercase { get; init; }

        public bool RequireNonAlphanumeric { get; init; }

        public bool RequireUppercase { get; init; }

        public int RequiredLength { get; init; }

        public int RequiredUniqueChars { get; init; }
    }

    public sealed class LockoutOption
    {
        public int DefaultLockoutTimeSpanInSecond { get; init; }

        public int MaxFailedAccessAttempts { get; init; }

        public bool AllowedForNewUsers { get; init; }
    }

    public sealed class UserOption
    {
        public string AllowedUserNameCharacters { get; init; }

        public bool RequireUniqueEmail { get; init; }
    }

    public sealed class SignInOption
    {
        public bool RequireConfirmedEmail { get; init; }

        public bool RequireConfirmedPhoneNumber { get; init; }
    }
}
