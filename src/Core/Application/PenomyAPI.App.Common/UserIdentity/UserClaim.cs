namespace PenomyAPI.App.Common.UserIdentity;

public sealed class UserClaim
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int ClaimTypeLength = 100;

        public const int ClaimValueLength = 100;
    }
    #endregion
}
