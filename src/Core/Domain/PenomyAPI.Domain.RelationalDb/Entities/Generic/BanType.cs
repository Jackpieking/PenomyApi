using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class BanType : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The system account that creates this ban type.
    /// </summary>
    public SystemAccount Creator { get; set; }

    public IEnumerable<UserBan> UserBans { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 100;

        public const int CodeLength = 32;

        public const int DescriptionLength = 500;
    }
    #endregion
}
