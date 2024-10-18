using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Common;
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
    private static readonly Expression<Func<DbContext, long, long, bool>> IsArtworkIsArtworkAvailableToDisplayById;
    private static readonly Func<DbContext, long, long, Task<bool>> IsArtworkAvailableToDisplayByIdCompileQuery;
    #endregion

    static ArtworkRepository()
    {
        IsArtworkIsArtworkAvailableToDisplayById = (DbContext dbContext, long artworkId, long userId) =>
            dbContext.Set<Artwork>()
                .Any(artwork =>
                    (
                        artwork.Id == artworkId
                        && artwork.PublicLevel == ArtworkPublicLevel.Everyone
                        && !artwork.IsTemporarilyRemoved
                        && !artwork.IsTakenDown
                    ) || (artwork.Id == artworkId && artwork.CreatedBy == userId)
                );

        IsArtworkAvailableToDisplayByIdCompileQuery = EF.CompileAsyncQuery(IsArtworkIsArtworkAvailableToDisplayById);
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
}
