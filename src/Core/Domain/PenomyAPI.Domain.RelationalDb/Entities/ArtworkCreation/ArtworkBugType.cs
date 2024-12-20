using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkBugType : EntityWithId<long>, ICreatedEntity<long>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public ArtworkBugSeverity BugSeverity { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The system account that create this bug type.
    /// </summary>
    public SystemAccount Creator { get; set; }

    public IEnumerable<ArtworkBugReport> BugReports { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;
    }
    #endregion
}
