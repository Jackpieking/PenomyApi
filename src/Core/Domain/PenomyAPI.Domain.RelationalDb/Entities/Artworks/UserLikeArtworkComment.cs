using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class UserLikeArtworkComment : IEntity
{
    public long CommentId { get; set; }

    public long UserId { get; set; }

    public DateTime LikedAt { get; set; }

    #region MetaData
    public static class MetaData { }
    #endregion
}
