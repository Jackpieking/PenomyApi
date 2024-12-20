using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPostReport : EntityWithId<long>, ICreatedEntity<long>
{
    public long PostId { get; set; }

    public long ReportProblemId { get; set; }

    public string DetailNote { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsResolved { get; set; }

    #region Navigation
    public UserPost UserPost { get; set; }

    public PostReportProblem ReportProblem { get; set; }

    public UserProfile Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;
    }
    #endregion
}
