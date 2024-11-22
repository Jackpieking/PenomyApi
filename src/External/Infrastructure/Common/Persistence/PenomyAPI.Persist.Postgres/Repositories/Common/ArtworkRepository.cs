using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Common;

internal sealed class ArtworkRepository : IArtworkRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Artwork> _artworkDbSet;

    #region Compiled Queries
    private static readonly Func<DbContext, long, long, Task<bool>> IsArtworkAvailableToDisplayByIdCompileQuery;
    #endregion

    static ArtworkRepository()
    {
        Expression<Func<DbContext, long, long, bool>> IsArtworkAvailableToDisplayByIdExpression = (DbContext dbContext, long artworkId, long userId) =>
            dbContext.Set<Artwork>()
                .Any(artwork =>
                    // Check if current artwork is public for everyone and not being removed or taken down.
                    (
                        artwork.Id == artworkId
                        && artwork.PublicLevel == ArtworkPublicLevel.Everyone
                        && !artwork.IsTemporarilyRemoved
                        && !artwork.IsTakenDown

                    )
                    // Or if the current artwork is published by the user with the id the same as the input.
                    || (artwork.Id == artworkId && artwork.CreatedBy == userId)
                );

        IsArtworkAvailableToDisplayByIdCompileQuery = EF.CompileAsyncQuery(
            IsArtworkAvailableToDisplayByIdExpression);
    }

    public ArtworkRepository(DbContext context)
    {
        _context = context;
        _artworkDbSet = context.Set<Artwork>();
    }

    public Task<bool> IsArtworkAvailableToDisplayByIdAsync(
        long artworkId,
        long userId,
        CancellationToken cancellationToken)
    {
        return IsArtworkAvailableToDisplayByIdCompileQuery.Invoke(_context, artworkId, userId);
    }

    public Task<bool> IsArtworkExistedByIdAsync(long artworkId, CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            artwork => artwork.Id == artworkId,
            cancellationToken);
    }

    public Task<string> GetChapterThumbnailDefaultUrlByArtworkIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet
            .AsNoTracking()
            .Where(artwork => artwork.Id == artworkId)
            .Select(artwork => artwork.ThumbnailUrl)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsArtworkBelongedToCreatorAsync(
        long artworkId,
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(artwork
            => artwork.Id == artworkId
            && artwork.CreatedBy == creatorId);
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

    public Task<int> GetLastChapterUploadOrderByArtworkIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet
            .Where(artwork => artwork.Id == artworkId)
            .Select(artwork => artwork.LastChapterUploadOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }
}