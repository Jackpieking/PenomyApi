using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPostComment : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public long PostId { get; set; }

    public bool IsDirectlyCommented { get; set; }

    public int TotalChildComments { get; set; }

    public long TotalLikes { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public UserPost UserPost { get; set; }

    public UserProfile Creator { get; set; }

    public IEnumerable<UserPostCommentAttachedMedia> AttachedMedias { get; set; }

    public IEnumerable<UserPostCommentParentChild> UserPostCommentParentChilds { get; set; }

    public IEnumerable<UserLikeUserPostComment> UserLikes { get; set; }

    public IEnumerable<UserPostCommentLikeStatistic> LikeStatistics { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
