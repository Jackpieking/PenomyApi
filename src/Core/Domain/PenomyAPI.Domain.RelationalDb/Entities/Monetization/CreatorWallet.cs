using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class CreatorWallet : EntityWithId<long>
{
    public long CreatorId { get; set; }

    public long BankId { get; set; }

    public string BankAccountNumber { get; set; }

    public CreatorWalletStatus WalletStatus { get; set; }

    public decimal WalletAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public CreatorProfile Creator { get; set; }

    public Bank Bank { get; set; }

    public IEnumerable<CreatorWalletTransaction> WalletTransactions { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int BankAccountNumberLength = 20;

        public const int WalletAmountPrecision = 18;

        public const int WalletAmountScale = 2;
    }
    #endregion
}
