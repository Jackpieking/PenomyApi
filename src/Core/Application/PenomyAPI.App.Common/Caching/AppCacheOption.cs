namespace PenomyAPI.App.Common.Caching;

public sealed class AppCacheOption
{
    private int _failSafeMaxDurationInSeconds;

    public int DurationInSeconds { get; set; }

    #region FailSafeOptionBlock
    // we set this flag to true
    public bool IsFailSafeEnabled { get; set; } = true;

    // we specify for how long a value should be usable,
    // even after its logical expiration
    public int FailSafeMaxDurationInSeconds
    {
        get => _failSafeMaxDurationInSeconds;
        set { _failSafeMaxDurationInSeconds = value != default ? value : DurationInSeconds; }
    }

    // we also specify for how long an expired value used
    // because of a fail-safe activation should be considered
    // temporarily non-expired, to avoid going to check the
    // database for every consecutive request of an expired
    // value
    public int FailSafeThrottleDurationInSeconds { get; set; } = 30;
    #endregion

    #region FactoryTimeout
    // Remember to handle SyntheticTimeoutException when factory hard
    // timeout is reached.

    // The maximum execution time allowed for the factory,
    // applied only if fail-safe is enabled and there is
    // a fallback value to return.
    public int FactorySoftTimeoutInSeconds { get; set; } = 100;

    // The maximum execution time allowed for the factory
    // in any case, even if there is not a stale value to
    // fall back to.
    public int FactoryHardTimeoutInSeconds { get; set; } = 1500;

    public bool AllowTimedOutFactoryBackgroundCompletion { get; set; } = true;
    #endregion
}
