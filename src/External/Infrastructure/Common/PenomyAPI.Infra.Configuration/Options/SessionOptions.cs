namespace PenomyAPI.Infra.Configuration.Options;

public sealed class SessionOptions
{
    public int IdleTimeoutInSecond { get; init; }

    public CookieOption Cookie { get; } = new();

    public sealed class CookieOption
    {
        public string Name { get; init; }

        public bool HttpOnly { get; init; }

        public bool IsEssential { get; init; }

        public int SecurePolicy { get; init; }

        public int SameSite { get; init; }
    }
}
