using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

/// <summary>
///     This class contain the reference of an artwork
///     that attach with the comment.
/// </summary>
public sealed class ArtworkCommentReference : IEntity
{
    public long CommentId { get; set; }

    public long ArtworkId { get; set; }

    #region Navigation
    public Artwork ReferencedArtwork { get; set; }

    public ArtworkComment Comment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
