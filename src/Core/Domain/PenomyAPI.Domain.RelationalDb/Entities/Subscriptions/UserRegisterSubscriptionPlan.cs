using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Subscriptions;

public sealed class UserRegisterSubscriptionPlan : EntityWithId<long>
{
    public long UserId { get; set; }

    public long SubscriptionPlanId { get; set; }

    /// <summary>
    ///     The price of the subscription at the time
    ///     the user subscribed. Value of the price may vary at different time.
    /// </summary>
    public decimal SubscribedPrice { get; set; }

    /// <summary>
    ///     Check if this user's subscription is still available or not.
    ///     If (true), the user is still available to access the subscription resources.
    /// </summary>
    public bool IsActive { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    #region Navigation
    public UserProfile Subscriber { get; set; }

    public SubscriptionPlan SubscriptionPlan { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int SubscribedPricePrecision = 18;

        /// <summary>
        ///     For more information about the decimal precision and scale,
        ///     please visit: https://4js.com/online_documentation/fjs-fgl-3.00.05-manual-html/c_fgl_datatypes_DECIMAL.html
        /// </summary>
        public const int SubscribedPriceScale = 2;
    }
    #endregion
}
