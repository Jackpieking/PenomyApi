using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class CreatorWalletTransactionType :
    EntityWithId<long>,
    ICreatedEntity<long>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public IEnumerable<CreatorWalletTransaction> WalletTransactions { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 100;

        public const int DescriptionLength = 200;
    }
    #endregion
}
