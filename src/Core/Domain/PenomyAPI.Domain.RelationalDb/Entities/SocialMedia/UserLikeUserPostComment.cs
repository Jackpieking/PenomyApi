using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserLikeUserPostComment : IEntity
{
    public long CommentId { get; set; }

    public long UserId { get; set; }

    public UserLikeValue Value { get; set; } = UserLikeValue.Like;

    public DateTime LikedAt { get; set; }

    #region Navigation
    public UserPostComment LikedComment { get; set; }

    public UserProfile User { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
