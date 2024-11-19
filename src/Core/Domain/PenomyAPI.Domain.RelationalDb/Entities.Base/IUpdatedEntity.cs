using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Base;

/// <summary>
///     Represent the base interface that all entity classes
///     that need to track the updating must inherit from.
/// </summary>
/// <typeparam name="TUpdatedBy">
///     The type of <see cref="UpdatedBy"/> property.
/// </typeparam>
public interface IUpdatedEntity<TUpdatedBy>
{
    TUpdatedBy UpdatedBy { get; set; }

    DateTime UpdatedAt { get; set; }
}
