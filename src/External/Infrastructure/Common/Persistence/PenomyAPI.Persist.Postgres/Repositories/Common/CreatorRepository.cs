using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Common;

internal sealed class CreatorRepository : ICreatorRepository
{
    private readonly DbContext _dbContext;

    public CreatorRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Compiled Queries section.

    #endregion

    public Task<bool> HasUserAlreadyBecomeCreatorAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<CreatorProfile>().AnyAsync(
            profile => profile.CreatorId == userId,
            cancellationToken);
    }
}
