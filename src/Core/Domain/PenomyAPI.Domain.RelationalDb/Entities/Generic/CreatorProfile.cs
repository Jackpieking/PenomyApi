using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class CreatorProfile : IEntity
{
    public long UserId { get; set; }

    public int TotalFollowers { get; set; }

    public int TotalArtworks { get; set; }

    public int ReportedCount { get; set; }

    /// <summary>
    ///     To store the date-time this user
    ///     confirmed to become a creator.
    /// </summary>
    public DateTime RegisteredAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The creator who has this profile.
    /// </summary>
    public UserProfile ProfileOwner { get; set; }

    public CreatorWallet CreatorWallet { get; set; }

    public IEnumerable<UserDonationTransaction> ReceivedUserDonations { get; set; }

    //public IEnumerable<ArtworkAppliedSubscriptionPlan> AppliedSubscriptionPlans { get; set; }

    public IEnumerable<ArtworkAppliedAdRevenueProgram> AppliedAdRevenuePrograms { get; set; }

    public IEnumerable<DonationThank> DonationThanks { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
