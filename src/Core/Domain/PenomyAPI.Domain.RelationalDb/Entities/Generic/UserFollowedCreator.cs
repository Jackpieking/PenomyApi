using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class UserFollowedCreator : IEntity
{
    public long UserId { get; set; }

    public long CreatorId { get; set; }

    public DateTime StartedAt { get; set; }

    #region Navigation
    public UserProfile User { get; set; }

    public UserProfile Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
