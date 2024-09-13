using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPostCommentLikeStatistic : IEntity
{
    public long CommentId { get; set; }

    public UserLikeValue Value { get; set; } = UserLikeValue.Like;

    /// <summary>
    ///     The total of a specific user like value.
    /// </summary>
    public long Total { get; set; }

    #region Navigation
    public GroupPostComment GroupPostComment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
