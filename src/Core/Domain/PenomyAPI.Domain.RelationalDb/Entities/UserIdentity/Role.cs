namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class Role
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string ConcurrencyStamp { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 100;

        public const int NormalizedNameLength = NameLength;

        public const int ConcurrencyStampLength = 64;
    }
    #endregion
}
