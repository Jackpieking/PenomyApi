using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public class UserProfileReportProblem : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public UserProfileReportProblemSeverity ProblemSeverity { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public IEnumerable<UserProfileReport> UserProfileReports { get; set; }

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
