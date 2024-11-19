using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Base;

/// <summary>
///     Represent the base interface that all entity classes
///     that need to track the creation must inherit from.
/// </summary>
/// <typeparam name="TCreatedBy">
///     The type of <see cref="CreatedBy"/> property.
/// </typeparam>
public interface ICreatedEntity<TCreatedBy>
{
    TCreatedBy CreatedBy { get; set; }

    DateTime CreatedAt { get; set; }
}
