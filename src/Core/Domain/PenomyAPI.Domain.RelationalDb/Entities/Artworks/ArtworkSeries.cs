using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkSeries : IEntity
{
    public long SeriesId { get; set; }

    public long ArtworkId { get; set; }

    public int ItemOrder { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }

    public Series Series { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
