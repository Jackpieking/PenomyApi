using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

public sealed class SystemRole : EntityWithId<long>
{
    /// <summary>
    ///     The name of this system role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The unique code of this system role
    ///     to differ from other role.
    /// </summary>
    /// <remarks>
    ///     Code will be documented clearly in SRS or ERD description.
    /// </remarks>
    public string Code { get; set; }

    public string Description { get; set; }

    #region Navigation
    public IEnumerable<SystemAccountRole> SystemAccountRoles { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 100;

        public const int CodeLength = 32;

        public const int DescriptionLength = 500;
    }
    #endregion
}
