using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art8Repository : IArt8Repository
{
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbContext _dbContext;

    public Art8Repository(DbContext context)
    {
        _dbContext = context;
        _artworkDbSet = context.Set<Artwork>();
    }

    public Task<bool> IsArtworkTemporarilyRemovedByIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            predicate: artwork
                => artwork.Id == artworkId
                && artwork.IsTemporarilyRemoved,
            cancellationToken: cancellationToken);
    }

    public async Task<bool> TemporarilyRemoveArtworkByIdAsync(
        long artworkId,
        long removedBy,
        DateTime removedAt,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalTemporarilyRemoveArtworkByIdAsync(
                    artworkId: artworkId,
                    removedBy: removedBy,
                    removedAt: removedAt,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalTemporarilyRemoveArtworkByIdAsync(
        long artworkId,
        long removedBy,
        DateTime removedAt,
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

            // Set the temporarily removed flag first.
            await _artworkDbSet
                .Where(predicate: updatedArtwork => updatedArtwork.Id == artworkId)
                .ExecuteUpdateAsync(
                    setPropertyCalls: updatedArtwork =>
                        updatedArtwork
                            .SetProperty(artwork => artwork.IsTemporarilyRemoved, true)
                            .SetProperty(artwork => artwork.TemporarilyRemovedBy, removedBy)
                            .SetProperty(artwork => artwork.TemporarilyRemovedAt, removedAt),
                    cancellationToken: cancellationToken
                );

            // Removed all related user's preference tables that related to this artwork.
            await _dbContext.Set<UserFollowedArtwork>()
                .Where(followedArtwork => followedArtwork.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.Set<UserFavoriteArtwork>()
                .Where(favoriteArtwork => favoriteArtwork.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.Set<UserArtworkViewHistory>()
                .Where(viewHistory => viewHistory.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.Set<GuestArtworkViewHistory>()
                .Where(viewHistory => viewHistory.ArtworkId == artworkId)
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
}
