using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkReportReason : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public ArtworkReportProblemSeverity ProblemSeverity { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public IEnumerable<ArtworkReport> ArtworkReports { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;
    }
    #endregion
}
