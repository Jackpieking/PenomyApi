using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkComment : EntityWithId<long>, ICreatedEntity<long>
{
    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }

    public string Content { get; set; }

    /// <summary>
    ///     Check if this comment is directly sent to the
    ///     comment section and not reply to another comment.
    /// </summary>
    public bool IsDirectlyCommented { get; set; }

    /// <summary>
    ///     The total number of child comments belonged to this comment.
    /// </summary>
    public int TotalChildComments { get; set; }

    public long TotalLikes { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public UserProfile Creator { get; set; }

    public IEnumerable<ArtworkCommentParentChild> ArtworkCommentParentChilds { get; set; }

    public IEnumerable<ArtworkCommentReference> ArtworkCommentReferences { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
