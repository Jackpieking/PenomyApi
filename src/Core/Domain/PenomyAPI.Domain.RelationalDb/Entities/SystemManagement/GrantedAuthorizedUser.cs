using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

public sealed class GrantedAuthorizedUser : IEntity
{
    /// <summary>
    ///     The system account's id that allowed to use
    ///     this authorized user account.
    /// </summary>
    public long GrantedTo { get; set; }

    /// <summary>
    ///     The user account's id that authorized by the system.
    /// </summary>
    public long AuthorizedUserId { get; set; }

    /// <summary>
    ///     The system account's id that provide the authorization.
    /// </summary>
    public long GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    /// <summary>
    ///     Check if the current authorized user is still available or not.
    /// </summary>
    /// <remarks>
    ///     true: The current authorized user is available. <br/>
    ///     false: The current authorized user is not permitted to access.
    /// </remarks>
    public bool IsActive { get; set; }

    #region Navigation
    /// <summary>
    ///     The system account that received
    ///     the authorized user account.
    /// </summary>
    public SystemAccount ReceivedSystemAccount { get; set; }

    /// <summary>
    ///     The system account that grants this
    ///     authorized user account.
    /// </summary>
    public SystemAccount GrantedSystemAccount { get; set; }

    /// <summary>
    ///     The user account that's authorized and
    ///     granted to other system account.
    /// </summary>
    public UserProfile AuthorizedUserAccount { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
