using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class UserFollowedArtwork : IEntity
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }

    public ArtworkType ArtworkType { get; set; }

    public DateTime StartedAt { get; set; }

    #region Navigation
    public Artwork FollowedArtwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
