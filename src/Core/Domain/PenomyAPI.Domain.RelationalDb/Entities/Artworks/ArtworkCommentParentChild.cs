using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkCommentParentChild : IEntity
{
    public long ParentCommentId { get; set; }

    public long ChildCommentId { get; set; }

    #region Navigation
    public ArtworkComment ParentComment { get; set; }

    public ArtworkComment ChildComment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
