using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Common;

internal sealed class DataSeedingRepository : IDataSeedingRepository
{
    private readonly DbContext _dbContext;

    public DataSeedingRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> HasSeedDataAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Set<PgRole>().AnyAsync(
            role => role.Id == UserRoles.GroupManager.Id,
            cancellationToken);
    }

    public async Task<bool> SeedDataAsync(CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalSeedDataAsync(cancellationToken: cancellationToken, result: result));

        return result.Value;
    }

    private async Task InternalSeedDataAsync(
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            // Seed the user roles.
            var userRoles = UserRoles.GetValues().Select(PgRole.MapFrom);

            await _dbContext.Set<PgRole>().AddRangeAsync(userRoles);

            // Seed the system roles.
            var systemRoles = SystemRoles.GetValues();

            await _dbContext.Set<SystemRole>().AddRangeAsync(systemRoles);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
}
