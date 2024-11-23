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

public sealed class G37Repository : IG37Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<CreatorProfile> _creatorProfileDbSet;

    public G37Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _creatorProfileDbSet = dbContext.Set<CreatorProfile>();
    }

    public Task<bool> HasUserAlreadyBecomeCreatorAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        return _creatorProfileDbSet.AnyAsync(
            profile => profile.CreatorId == userId,
            cancellationToken);
    }

    public async Task<bool> RegisterUserAsCreatorAsync(long userId, CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalRegisterUserAsCreatorAsync(
                    userId,
                    cancellationToken,
                    result
                )
        );

        return result.Value;
    }

    private async Task InternalRegisterUserAsCreatorAsync(
        long userId, CancellationToken cancellationToken, Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            var userProfileDbSet = _dbContext.Set<UserProfile>();

            await userProfileDbSet
                .Where(profile => profile.UserId == userId)
                .ExecuteUpdateAsync(
                    setPropertyCalls: profile => profile
                        .SetProperty(
                            profile => profile.RegisterAsCreator,
                            profile => true),
                    cancellationToken: cancellationToken);

            // Create the creator profile for current user.
            var newCreatorProfile = new CreatorProfile
            {
                CreatorId = userId,
                TotalArtworks = 0,
                TotalFollowers = 0,
                RegisteredAt = DateTime.UtcNow,
            };

            await _creatorProfileDbSet.AddAsync(newCreatorProfile, cancellationToken);

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
