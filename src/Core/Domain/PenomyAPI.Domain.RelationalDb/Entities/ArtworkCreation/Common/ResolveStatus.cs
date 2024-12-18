namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

/// <summary>
///     The status of resolving report or problem.
/// </summary>
public enum ResolveStatus
{
    /// <summary>
    ///     The problem is just created and pending to resolve.
    /// </summary>
    Pending = 1,

    /// <summary>
    ///     The problem is received by the resolver but not resolve.
    /// </summary>
    Received = 2,

    /// <summary>
    ///     The problem is received and being processing by the resolver.
    /// </summary>
    InProcessing = 3,

    /// <summary>
    ///     The problem is resolved by the creator and accepted by the user.
    /// </summary>
    Done = 4,

    /// <summary>
    ///     The problem is finished and closed.
    /// </summary>
    Closed = 5,
}
