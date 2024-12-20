using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkCollaboratedRequest : EntityWithId<long>, ICreatedEntity<long>
{
    public long CreatedBy { get; set; }

    public long ArtworkId { get; set; }

    public long RoleId { get; set; }

    public ArtworkCollaboratedRequestStatus RequestStatus { get; set; }

    public string RequestNote { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public CreatorProfile Creator { get; set; }

    public Artwork RequestedArtwork { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int RequestNoteLength = 2000;
    }
    #endregion
}
