using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatGroupMember : IEntity
{
    public long MemberId { get; set; }

    public long ChatGroupId { get; set; }

    public long RoleId { get; set; }

    public DateTime JoinedAt { get; set; }

    #region Navigation
    public UserProfile Member { get; set; }

    public ChatGroup ChatGroup { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
