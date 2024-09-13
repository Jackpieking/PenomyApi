using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class UserRatingArtwork : IEntity
{
    public long ArtworkId { get; set; }

    public long UserId { get; set; }

    public byte StarRates { get; set; }

    public DateTime RatedAt { get; set; }

    #region Navigation
    public Artwork RatedArtwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
