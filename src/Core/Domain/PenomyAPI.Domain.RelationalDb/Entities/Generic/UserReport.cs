using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class UserReport : EntityWithId<long>, ICreatedEntity<long>
{
    /// <summary>
    ///     Id of the user who received the reports.
    /// </summary>
    public long ReportedUserId { get; set; }

    public long ReportProblemId { get; set; }

    public string DetailNote { get; set; }

    /// <summary>
    ///     Id of the user who creates the reports.
    /// </summary>
    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The user that being reported.
    /// </summary>
    public UserProfile ReportedUser { get; set; }

    public UserReportProblem ReportProblem { get; set; }

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
