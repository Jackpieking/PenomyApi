using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public sealed class Bank :
    EntityWithId<long>,
    ICreatedEntity<long>,
    IUpdatedEntity<long>
{
    /// <summary>
    ///     This bankId is used to store the bankId retrieved from 3rd party api.
    ///     Reference: https://www.vietqr.io/danh-sach-api/api-danh-sach-ma-ngan-hang/
    /// </summary>
    public int BankId { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Bin { get; set; }

    public string ShortName { get; set; }

    public string LogoUrl { get; set; }

    public string SwiftCode { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public IEnumerable<CreatorWallet> CreatorWallets { get; set; }

    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        /// <summary>
        ///     This bankId is used to store the bankId retrieved from 3rd party api.
        ///     Reference: https://www.vietqr.io/danh-sach-api/api-danh-sach-ma-ngan-hang/
        /// </summary>
        public const int BankIdLength = 100;

        public const int NameLength = 200;

        public const int CodeLength = 16;

        public const int BinLength = 8;
        
        public const int ShortNameLength = 64;
        
        public const int LogoUrlLength = 2000;
        
        public const int SwiftCodeLength = 32;
    }
    #endregion
}
