using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class BugReport : EntityWithId<long>, ICreatedEntity<long>
{
    public long CreatedBy { get; set; }

    public long BugTypeId { get; set; }

    /// <summary>
    ///     The title of this bug report.
    /// </summary>
    /// <remark>
    ///     User can see this part of the bug report.
    /// </remark>
    public string Title { get; set; }

    /// <summary>
    ///     The overview description of this bug report.
    /// </summary>
    /// <remark>
    ///     Only admin can see this part of the bug report.
    /// </remark>
    public string Overview { get; set; }

    public string UserDetailNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsResolved { get; set; }

    public string ResolveNote { get; set; }

    public long ResolvedBy { get; set; }

    public DateTime ResolvedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The user who create this bug report.
    /// </summary>
    public UserProfile Creator { get; set; }

    public BugType BugType { get; set; }

    public IEnumerable<BugReportAttachedMedia> AttachedMedias { get; set; }

    public SystemAccount Resolver { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 500;

        public const int OverviewLength = 1000;

        public const int UserDetailNoteLength = 2000;

        public const int ResolveNoteLength = 2000;
    }
    #endregion
}
