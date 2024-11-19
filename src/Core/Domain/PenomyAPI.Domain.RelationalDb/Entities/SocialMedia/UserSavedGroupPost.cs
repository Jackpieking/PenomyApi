using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserSavedGroupPost : IEntity
{
    public long UserId { get; set; }

    public long PostId { get; set; }

    public DateTime SavedAt { get; set; }

    #region Navigation
    public UserProfile User { get; set; }

    public GroupPost GroupPost { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
