using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserFriend : IEntity
{
    public long UserId { get; set; }

    public long FriendId { get; set; }

    public DateTime StartedAt { get; set; }

    #region MetaData
    public static class MetaData { }
    #endregion
}
