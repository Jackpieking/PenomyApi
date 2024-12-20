using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPostComment : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public long PostId { get; set; }

    public int TotalChildComments { get; set; }

    public bool IsDirectlyCommented { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsRemoved { get; set; }

    #region Navigation
    public GroupPost GroupPost { get; set; }

    public UserProfile Creator { get; set; }

    public IEnumerable<GroupPostCommentAttachedMedia> AttachedMedias { get; set; }

    public IEnumerable<UserLikeGroupPostComment> UserLikes { get; set; }

    public IEnumerable<GroupPostCommentParentChild> GroupPostCommentParentChilds { get; set; }

    public IEnumerable<GroupPostCommentLikeStatistic> LikeStatistics { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
