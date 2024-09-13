using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPostCommentParentChild : IEntity
{
    public long ParentCommentId { get; set; }

    public long ChildCommentId { get; set; }

    #region Navigation
    public UserPostComment ParentComment { get; set; }

    public UserPostComment ChildComment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
