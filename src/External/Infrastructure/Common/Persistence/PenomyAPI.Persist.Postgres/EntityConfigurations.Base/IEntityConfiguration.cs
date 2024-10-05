using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Persist.Postgres.EntityConfigurations.Base;

internal interface IEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{ }
