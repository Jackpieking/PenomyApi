using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPostLikeStatistic : IEntity
{
    public long PostId { get; set; }

    public long ValueId { get; set; }

    /// <summary>
    ///     The total of a specific user like value.
    /// </summary>
    public long Total { get; set; }

    public static UserPostLikeStatistic Empty(long postId)
    {
        return new UserPostLikeStatistic { PostId = postId, ValueId = 0, Total = 0 };
    }

    #region MetaData

    public static class MetaData
    {
    }

    #endregion

    #region Navigation

    public UserPost UserPost { get; set; }

    public UserLikeValue LikeValue { get; set; }

    #endregion
}
