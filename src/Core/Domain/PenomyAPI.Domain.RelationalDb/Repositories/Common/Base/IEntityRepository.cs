using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Base;

/// <summary>
///     The base interface for the common entity repositories.
/// </summary>
/// <typeparam name="TEntity">
///     The entity this repository will interact with.
/// </typeparam>
public interface IEntityRepository<TEntity>
    where TEntity : class, IEntity
{
}
