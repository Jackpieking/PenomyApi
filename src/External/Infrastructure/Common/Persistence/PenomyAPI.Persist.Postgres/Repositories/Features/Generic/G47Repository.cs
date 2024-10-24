using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G47Repository : IG47Repository
{
    private readonly AppDbContext _context;
    private readonly DbSet<UserFavoriteArtwork> _artworkFavoriteContext;
    public G47Repository(AppDbContext context)
    {
        _context = context;
        _artworkFavoriteContext = context.Set<UserFavoriteArtwork>();
    }
    public Task<bool> RemoveFromFavoriteAsync(long userId, long artworkId, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }
}
