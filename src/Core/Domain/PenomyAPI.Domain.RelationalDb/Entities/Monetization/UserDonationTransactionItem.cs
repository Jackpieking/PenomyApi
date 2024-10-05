using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class UserDonationTransactionItem : IEntity
{
    public long DonationTransactionId { get; set; }

    public long DonationItemId { get; set; }

    public decimal ItemPrice { get; set; }

    public int ItemQuantity { get; set; }

    #region Navigation
    public UserDonationTransaction DonationTransaction { get; set; }

    public DonationItem DonationItem { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
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
