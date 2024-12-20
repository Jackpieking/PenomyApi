using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroupReport : EntityWithId<long>, ICreatedEntity<long>
{
    public long GroupId { get; set; }

    public long ReportProblemId { get; set; }

    public string DetailNote { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public SocialGroupReportProblem ReportProblem { get; set; }

    public UserProfile Reporter { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int DetailNoteLength = 2000;
    }
    #endregion
}
