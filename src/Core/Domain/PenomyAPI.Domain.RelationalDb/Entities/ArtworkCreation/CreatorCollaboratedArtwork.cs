using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class CreatorCollaboratedArtwork : IEntity
{
    public long CreatorId { get; set; }

    public long ArtworkId { get; set; }

    public long RoleId { get; set; }

    public long GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The creator that's granted to collaborate
    ///     on this artwork with a specified role.
    /// </summary>
    public CreatorProfile GrantedCreator { get; set; }

    public Artwork ManagedArtwork { get; set; }

    /// <summary>
    ///     The creator who decides to provider permission
    ///     to other creator to collaborate on this artwork.
    /// </summary>
    public CreatorProfile PermissionGrantProvider { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
