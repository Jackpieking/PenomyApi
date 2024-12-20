using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

public sealed class UserLikeValue : EntityWithId<long>, ICreatedEntity<long>
{
    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public bool ForDefaultDisplay { get; set; }

    public string Value { get; set; }

    public string EmojiUrl { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public IEnumerable<UserPostLikeStatistic> UserPostLikeStatistics { get; set; }

    public IEnumerable<UserPostCommentLikeStatistic> UserPostCommentLikeStatistics { get; set; }

    public IEnumerable<GroupPostLikeStatistic> GroupPostLikeStatistics { get; set; }

    public IEnumerable<GroupPostCommentLikeStatistic> GroupPostCommentLikeStatistics { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 64;

        public const int ValueLength = 100;

        public const int EmojiUrlLength = 2000;
    }
    #endregion
}
