using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkReport : EntityWithId<long>, ICreatedEntity<long>
{
    public long ArtworkId { get; set; }

    public long ReportProblemId { get; set; }

    public string DetailNote { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public ResolveStatus ResolveStatus { get; set; }

    public string ResolveNote { get; set; }

    public long ResolvedBy { get; set; }

    public DateTime ResolvedAt { get; set; }

    #region Navigation
    public Artwork ReportedArtwork { get; set; }

    public ArtworkReportProblem ReportedProblem { get; set; }

    public UserProfile Creator { get; set; }

    public CreatorProfile Resolver { get; set; }

    public IEnumerable<ArtworkReportAttachedMedia> AttachedMedias { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;

        public const int ResolveNoteLength = 2000;
    }
    #endregion
}
