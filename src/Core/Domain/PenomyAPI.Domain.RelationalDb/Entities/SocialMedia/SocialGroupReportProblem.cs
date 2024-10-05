using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroupReportProblem : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public SocialGroupReportProblemSeverity ProblemSeverity { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public IEnumerable<SocialGroupReport> SocialGroupReports { get; set; }

    public SystemAccount Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;
    }
    #endregion
}
