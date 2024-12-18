using System.ComponentModel.DataAnnotations.Schema;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPostLikeStatistic : IEntity
{
    public long PostId { get; set; }

    public long ValueId { get; set; }

    /// <summary>
    ///     The total of a specific user like value.
    /// </summary>
    public long Total { get; set; }

    public static GroupPostLikeStatistic Empty(long postId)
    {
        return new GroupPostLikeStatistic
        {
            PostId = postId,
            ValueId = 0,
            Total = 0,
        };
    }

    #region Navigation
    public GroupPost GroupPost { get; set; }

    [NotMapped]
    public UserLikeValue LikeValue { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
