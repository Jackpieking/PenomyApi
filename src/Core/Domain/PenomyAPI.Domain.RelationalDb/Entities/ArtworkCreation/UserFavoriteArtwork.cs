using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class UserFavoriteArtwork : IEntity
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }

    public DateTime StartedAt { get; set; }

    #region Navigation
    public Artwork FavoriteArtwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
