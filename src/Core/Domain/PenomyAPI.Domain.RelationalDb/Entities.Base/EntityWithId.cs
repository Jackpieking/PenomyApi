using System.ComponentModel.DataAnnotations.Schema;

namespace PenomyAPI.Domain.RelationalDb.Entities.Base;

/// <summary>
///     The base class for any entity that have id as a primary key to extends from.
/// </summary>
/// <typeparam name="TEntityId">
///     The type of the id (primary key).
/// </typeparam>
public abstract class EntityWithId<TEntityId> : IEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public TEntityId Id { get; set; }
}
