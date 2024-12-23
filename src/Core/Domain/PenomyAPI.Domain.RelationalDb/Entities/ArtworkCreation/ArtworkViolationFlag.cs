using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkViolationFlag : EntityWithId<long>, ICreatedEntity<long>
{
    public long ViolationFlagTypeId { get; set; }

    public long ArtworkId { get; set; }

    public string DetailNote { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public ResolveStatus ResolveStatus { get; set; }

    public string ResolveNote { get; set; }

    public long ResolvedBy { get; set; }

    public DateTime ResolvedAt { get; set; }

    #region Navigation
    public ArtworkViolationFlagType ViolationFlagType { get; set; }

    public Artwork ViolatedArtwork { get; set; }

    public SystemAccount Creator { get; set; }

    public CreatorProfile Resolver { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;

        public const int ResolveNoteLength = 2000;
    }
    #endregion
}
