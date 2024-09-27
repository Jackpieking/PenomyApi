namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class UserLogin
{
    public string LoginProvider { get; set; }

    public string ProviderKey { get; set; }

    public long UserId { get; set; }

    public string ProviderDisplayName { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int LoginProviderLength = 100;

        public const int ProviderKeyLength = 200;

        public const int ProviderDisplayNameLength = 200;
    }
    #endregion
}
