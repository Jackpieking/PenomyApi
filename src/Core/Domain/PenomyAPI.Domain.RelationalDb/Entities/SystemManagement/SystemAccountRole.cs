using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

public sealed class SystemAccountRole : IEntity
{
    public long SystemAccountId { get; set; }

    public long RoleId { get; set; }

    #region Navigation
    public SystemAccount SystemAccount { get; set; }

    public SystemRole SystemRole { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
