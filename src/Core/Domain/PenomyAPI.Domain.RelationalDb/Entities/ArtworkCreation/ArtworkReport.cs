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

    #region Navigation
    public Artwork ReportedArtwork { get; set; }

    public ArtworkReportProblem ReportedProblem { get; set; }

    public UserProfile Creator { get; set; }

    public IEnumerable<ArtworkReportAttachedMedia> AttachedMedias { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;
    }
    #endregion
}
