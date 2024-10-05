using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class UserProfileReport : EntityWithId<long>, ICreatedEntity<long>
{
    /// <summary>
    ///     Id of the user profile that received the reports.
    /// </summary>
    public long ReportedProfileId { get; set; }

    public long ReportProblemId { get; set; }

    public string DetailNote { get; set; }

    /// <summary>
    ///     Id of the user who creates the reports.
    /// </summary>
    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The user profile that being reported.
    /// </summary>
    public UserProfile ReportedUserProfile { get; set; }

    public UserProfileReportProblem ReportProblem { get; set; }

    /// <summary>
    ///     The user that creates this report.
    /// </summary>
    public UserProfile Reporter { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;
    }
    #endregion
}
