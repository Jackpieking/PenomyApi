using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G54Repository : IG54Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<ArtworkCommentParentChild> _commentParentChildDbSet;

    public G54Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _commentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
    }

    public async Task<bool> RemoveCommentAsync(long CommentId)
    {
        try
        {
            await _artworkCommentDbSet
                .Where(c => c.Id == CommentId)
                .ExecuteUpdateAsync(c => c.SetProperty(c => c.IsRemoved, true));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
