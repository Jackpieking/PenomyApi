using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

/// <summary>
///     This table is supported for fan of a specific artwork
///     can visit the fan group easier.
/// </summary>
public sealed class SocialGroupRelatedArtwork : IEntity
{
    public long GroupId { get; set; }

    public long ArtworkId { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public Artwork Artwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
