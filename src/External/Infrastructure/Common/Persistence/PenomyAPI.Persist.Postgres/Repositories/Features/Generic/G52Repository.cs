using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G52Repository : IG52Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;

    public G52Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
    }

    public async Task<long> CreateCommentAsync(ArtworkComment Comment)
    {
        try
        {
            _artworkCommentDbSet.Add(Comment);
            await _dbContext.SaveChangesAsync();
            return Comment.Id;
        }
        catch
        {
            return 0;
        }
    }
}
