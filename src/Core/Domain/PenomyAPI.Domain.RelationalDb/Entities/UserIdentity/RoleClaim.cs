namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class RoleClaim
{
    public int Id { get; set; }

    public long RoleId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public static class MetaData
    {
        public const int ClaimTypeLength = 100;

        public const int ClaimValueLength = 100;
    }
}
