using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class UserChatGroupActiveHistory : IEntity
{
    public long UserId { get; set; }

    public long ChatGroupId { get; set; }

    /// <summary>
    ///     Store the last time the user has any
    ///     interaction in a specific joined group.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    #region Navigation
    public UserProfile User { get; set; }

    public ChatGroup ChatGroup { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
