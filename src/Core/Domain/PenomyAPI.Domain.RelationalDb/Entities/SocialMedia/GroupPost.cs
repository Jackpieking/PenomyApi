using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPost : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public long TotalLikes { get; set; }

    public long GroupId { get; set; }

    public bool AllowComment { get; set; }

    public bool IsApproved { get; set; }

    public long ApprovedBy { get; set; }

    public DateTime ApprovedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public UserProfile Approver { get; set; }

    public UserProfile Creator { get; set; }

    public IEnumerable<GroupPostAttachedMedia> AttachedMedias { get; set; }

    public IEnumerable<UserLikeGroupPost> UserLikes { get; set; }

    public IEnumerable<GroupPostComment> Comments { get; set; }

    public IEnumerable<GroupPostReport> GroupPostReports { get; set; }

    public IEnumerable<GroupPostLikeStatistic> LikeStatistics { get; set; }

    public GroupPinnedPost GroupPinnedPost { get; set; }

    public IEnumerable<UserSavedGroupPost> UserSavedGroupPosts { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
