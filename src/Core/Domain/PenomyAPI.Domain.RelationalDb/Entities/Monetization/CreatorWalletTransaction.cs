using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization.Common;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class CreatorWalletTransaction : EntityWithId<long>
{
    public long WalletId { get; set; }

    public long TransactionTypeId { get; set; }

    public string TransactionCode { get; set; }

    public AppTransactionStatus TransactionStatus { get; set; }

    public decimal TransactionAmount { get; set; }

    public string TransactionMetaData { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public CreatorWalletTransactionType TransactionType { get; set; }

    public CreatorWallet CreatorWallet { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TransactionCodeLength = 64;

        public const int TransactionMetaDataLength = 320;

        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int TransactionAmountPrecision = 18;

        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int TransactionAmountScale = 2;
    }
    #endregion
}
