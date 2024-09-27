namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class UserRole
{
    public long UserId { get; set; }

    public long RoleId { get; set; }

    #region MetaData
    public static class MetaData { }
    #endregion
}
