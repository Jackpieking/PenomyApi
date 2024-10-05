using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class UserManagedArtwork : IEntity
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }

    public long RoleId { get; set; }

    public long GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The user that's granted to collaborate
    ///     on this artwork with a specified role.
    /// </summary>
    public UserProfile GrantedUser { get; set; }

    public Artwork ManagedArtwork { get; set; }

    /// <summary>
    ///     The user who mainly manages and decides
    ///     to invite other user to collaborate on this artwork.
    /// </summary>
    public UserProfile ArtworkManager { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
