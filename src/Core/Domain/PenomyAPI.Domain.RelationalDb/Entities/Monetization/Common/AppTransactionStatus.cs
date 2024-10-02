namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization.Common;

/// <summary>
///     Store the enum of transaction status in this
///     system that supported when working with money.
/// </summary>
public enum AppTransactionStatus
{
    /// <summary>
    ///     This status indicates the transaction is being
    ///     processed by the system.
    /// </summary>
    InProcessing = 1,

    /// <summary>
    ///     This status indicates the transaction is successfully
    ///     processed by the system and persisted to database permanently.
    /// </summary>
    Success = 2,

    /// <summary>
    ///     This status indicates the transaction is failed
    ///     when being processed by the system and persisted to
    ///     support for tracing problem.
    /// </summary>
    Failed = 3,
}
