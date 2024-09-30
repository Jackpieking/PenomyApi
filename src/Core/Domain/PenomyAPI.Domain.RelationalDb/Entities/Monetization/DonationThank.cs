using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class DonationThank : IEntity
{
    public long UserDonationTransactionId { get; set; }

    public long CreatorId { get; set; }

    public string ThankNote { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Relationships
    public UserDonationTransaction UserDonationTransaction { get; set; }

    public CreatorProfile Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ThankNoteLength = 640;
    }
    #endregion
}
