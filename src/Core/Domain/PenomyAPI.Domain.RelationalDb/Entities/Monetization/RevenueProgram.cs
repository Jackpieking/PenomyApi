using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class RevenueProgram :
    EntityWithId<long>,
    ICreatedEntity<long>,
    IUpdatedEntity<long>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public int MinTotalViewsToApply { get; set; }

    public int MinTotalFollowersToApply { get; set; }

    public int MinTotalFavoritesToApply { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 1000;
    }
    #endregion
}
