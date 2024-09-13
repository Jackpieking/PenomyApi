using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.Subscriptions;

public sealed class SubscriptionPlan : EntityWithId<long>
{
    public string PlanName { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    /// <summary>
    ///     Represent the duration of the subscription measured in days.
    /// </summary>
    public int DurationInDays { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public IEnumerable<UserRegisterSubscriptionPlan> UserRegisterSubscriptionPlans { get; set; }

    public SystemAccount Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int PlanNameLength = 100;

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

        public const int DescriptionLength = 500;
    }
    #endregion
}
