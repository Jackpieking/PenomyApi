using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPost : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public long TotalLikes { get; set; }

    public UserPostPublicLevel PublicLevel { get; set; } = UserPostPublicLevel.OnlyMe;

    public bool AllowComment { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public UserProfile Creator { get; set; }

    public IEnumerable<UserPostReport> UserPostReports { get; set; }

    public IEnumerable<UserSavedUserPost> UserSavedUserPosts { get; set; }

    public IEnumerable<UserLikeUserPost> UserLikes { get; set; }

    public IEnumerable<UserPostAttachedMedia> AttachedMedias { get; set; }

    public IEnumerable<UserPostComment> Comments { get; set; }

    public IEnumerable<UserPostLikeStatistic> LikeStatistics { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
