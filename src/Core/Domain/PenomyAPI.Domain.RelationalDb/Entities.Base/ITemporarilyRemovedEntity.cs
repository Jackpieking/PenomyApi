using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Base;

/// <summary>
///     Represent the base interface that all entity classes
///     that need to track the temporarily removed must inherit from.
/// </summary>
/// <typeparam name="TTemporarilyRemovedBy">
///     The type of <see cref="TemporarilyRemovedBy"/> property.
/// </typeparam>
public interface ITemporarilyRemovedEntity<TTemporarilyRemovedBy>
{
    TTemporarilyRemovedBy TemporarilyRemovedBy { get; set; }

    DateTime TemporarilyRemovedAt { get; set; }

    bool IsTemporarilyRemoved { get; set; }
}
