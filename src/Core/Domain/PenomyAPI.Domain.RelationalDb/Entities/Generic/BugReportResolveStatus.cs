﻿namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

/// <summary>
///     The status of resolving report or problem.
/// </summary>
public enum BugReportResolveStatus
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
    ///     The problem is finished and closed.
    /// </summary>
    Closed = 4,
}
