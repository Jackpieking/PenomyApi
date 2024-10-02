using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization.Common;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization
{
    public sealed class UserDonationTransaction : EntityWithId<long>
    {
        public long DonatorId { get; set; }

        public long CreatorId { get; set; }

        public long WalletTransactionId { get; set; }

        public string TransactionCode { get; set; }

        public AppTransactionStatus TransactionStatus { get; set; }

        public decimal TotalDonationAmount { get; set; }

        public int CreatorReceivedPercentage { get; set; }

        public bool HasReceivedThankFromCreator { get; set; }

        public string DonationNote { get; set; }

        public DateTime CreatedAt { get; set; }

        #region Navigation
        public UserProfile Donator { get; set; }

        public CreatorProfile Creator { get; set; }

        public CreatorWalletTransaction WalletTransaction { get; set; }

        public IEnumerable<UserDonationTransactionItem> TransactionItems { get; set; }

        public DonationThank DonationThank { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const int TransactionCodeLength = 64;

            public const int DonationNoteLength = 1000;

            /// <summary>
            ///     For more information about the decimal precision and scale,
            ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
            /// </summary>
            public const int DonationAmountPrecision = 18;

            /// <summary>
            ///     For more information about the decimal precision and scale,
            ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
            /// </summary>
            public const int DonationAmountScale = 2;
        }
        #endregion
    }
}
