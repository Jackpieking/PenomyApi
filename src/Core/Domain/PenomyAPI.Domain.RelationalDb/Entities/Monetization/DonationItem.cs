using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class DonationItem : EntityWithId<long>, ICreatedEntity<long>, IUpdatedEntity<long>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public bool AllowDonatorToSetPrice { get; set; }

    public int CreatorReceivedPercentage { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }

    public IEnumerable<UserDonationTransactionItem> DonationTransactionItems { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 100;

        public const int DescriptionLength = 500;

        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int PricePrecision = 18;

        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int PriceScale = 2;
    }
    #endregion
}
