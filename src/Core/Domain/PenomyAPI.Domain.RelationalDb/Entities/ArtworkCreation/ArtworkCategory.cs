using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

/// <summary>
///     The entity that persist the list of categories of a specific artwork.
/// </summary>
public sealed class ArtworkCategory : IEntity
{
    public long ArtworkId { get; set; }

    public long CategoryId { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }

    public Category Category { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
