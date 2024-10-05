using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class ArtworkAppliedAdRevenueProgram : IEntity
{
    public long ArtworkId { get; set; }

    public long AdRevenueProgramId { get; set; }

    public AppliedStatus AppliedStatus { get; set; }

    public long ProposedBy { get; set; }

    public DateTime ProposedAt { get; set; }

    public long ApprovedBy { get; set; }

    public DateTime ApprovedAt { get; set; }

    public string RejectedNote { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }

    public CreatorProfile Proposer { get; set; }

    public SystemAccount Approver { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int RejectedNoteLength = 1000;
    }
    #endregion
}
