namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization;

public enum CreatorWalletStatus
{
    /// <summary>
    ///     Wallet with this status is waiting for the
    ///     creator to confirm with system about their wallet information.
    /// </summary>
    PendingConfirmed = 0,

    /// <summary>
    ///     Wallet with this status has confirmed successfully
    ///     and ready to receive and withdraw money.
    /// </summary>
    Active = 1,

    /// <summary>
    ///     Wallet with this status is locked by the creator
    ///     or the system for some reasons to prevent bad actors take advantages.
    /// </summary>
    Locked = 2,
}
