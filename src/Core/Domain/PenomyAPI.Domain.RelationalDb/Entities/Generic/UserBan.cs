using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class UserBan : EntityWithId<long>
{
    public long UserId { get; set; }

    public long BanTypeId { get; set; }

    public long BannedBy { get; set; }

    /// <summary>
    ///     Check if the current user's ban is still available or not.
    ///     If the ban is inactive, user will no longer be restricted.
    /// </summary>
    public bool IsActive { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    #region Navigation
    public UserProfile BannedUser { get; set; }

    public BanType BanType { get; set; }

    /// <summary>
    ///     The system account that ban this user.
    /// </summary>
    public SystemAccount Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
