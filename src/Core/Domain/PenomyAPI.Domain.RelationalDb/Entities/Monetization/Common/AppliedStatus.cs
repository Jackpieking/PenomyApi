namespace PenomyAPI.Domain.RelationalDb.Entities.Monetization.Common;

public enum AppliedStatus
{
    /// <summary>
    ///     Status indicate to wait for the artwork manager to approve the application.
    /// </summary>
    Pending = 0,

    /// <summary>
    ///     Status indicate that the application is approved.
    /// </summary>
    Approved = 1,


    /// <summary>
    ///     Status indicate that the application is rejected.
    /// </summary>
    Rejected = 2,
}
