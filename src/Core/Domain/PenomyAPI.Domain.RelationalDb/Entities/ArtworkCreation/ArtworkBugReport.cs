using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkBugReport : EntityWithId<long>, ICreatedEntity<long>
{
    public long CreatedBy { get; set; }

    public long BugTypeId { get; set; }

    public long ArtworkId { get; set; }

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
    ///     Both creator and admin can see this part of the bug report.
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

    public Artwork Artwork { get; set; }

    public ArtworkBugType BugType { get; set; }

    public IEnumerable<ArtworkBugReportAttachedMedia> AttachedMedias { get; set; }

    public SystemAccount Resolver { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 640;

        public const int OverviewLength = 1000;

        public const int UserDetailNoteLength = 2000;

        public const int ResolveNoteLength = 2000;
    }
    #endregion
}
