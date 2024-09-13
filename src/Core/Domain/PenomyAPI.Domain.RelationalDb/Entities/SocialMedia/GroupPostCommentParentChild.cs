using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPostCommentParentChild : IEntity
{
    public long ParentCommentId { get; set; }

    public long ChildCommentId { get; set; }

    #region Navigation
    public GroupPostComment ParentComment { get; set; }

    public GroupPostComment ChildComment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
