using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserLikeUserPost : IEntity
{
    public long PostId { get; set; }

    public long UserId { get; set; }

    public long ValueId { get; set; }

    public DateTime LikedAt { get; set; }

    #region Navigation
    public UserPost UserPost { get; set; }

    public UserProfile User { get; set; }

    public UserLikeValue LikeValue { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
