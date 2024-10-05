using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkReportProblem : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public ArtworkReportProblemSeverity ProblemSeverity { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public IEnumerable<ArtworkReport> ArtworkReports { get; set; }

    public IEnumerable<ArtworkChapterReport> ArtworkChapterReports { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;
    }
    #endregion
}
