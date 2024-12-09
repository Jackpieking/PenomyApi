using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G36Repository : IG36Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserProfile> _profileDbSet;

    public G36Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _profileDbSet = dbContext.Set<UserProfile>();
    }

    public Task<bool> IsNickNameAlreadyExistedAsync(
        long userId,
        string nickName,
        CancellationToken cancellationToken)
    {
        return _profileDbSet.AnyAsync(
            profile => profile.NickName.Equals(nickName)
                && profile.UserId != userId,
            cancellationToken);
    }

    public async Task<bool> UpdateProfileAsync(
        UserProfile userProfile,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalUpdateProfileAsync(
                    userProfileToUpdate: userProfile,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdateProfileAsync(
        UserProfile userProfileToUpdate,
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

            var updatedAt = DateTime.UtcNow;

            await _profileDbSet
                .Where(profile => profile.UserId == userProfileToUpdate.UserId)
                .ExecuteUpdateAsync(
                    profile => profile
                        .SetProperty(
                            profile => profile.NickName,
                            profile => userProfileToUpdate.NickName)
                        .SetProperty(
                            profile => profile.AboutMe,
                            profile => userProfileToUpdate.AboutMe)
                        .SetProperty(
                            profile => profile.UpdatedAt,
                            profile => updatedAt),
                    cancellationToken);

            if (!Equals(userProfileToUpdate.AvatarUrl, null))
            {
                await _profileDbSet
                .Where(profile => profile.UserId == userProfileToUpdate.UserId)
                .ExecuteUpdateAsync(
                    profile => profile
                        .SetProperty(
                            profile => profile.AvatarUrl,
                            profile => userProfileToUpdate.AvatarUrl),
                    cancellationToken);
            }

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
