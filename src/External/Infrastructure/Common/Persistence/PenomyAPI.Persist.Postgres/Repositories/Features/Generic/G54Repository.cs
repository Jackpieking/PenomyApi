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
    private readonly DbSet<ArtworkCommentReference> _commentReferenceDbSet;

    public G54Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _commentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _commentReferenceDbSet = dbContext.Set<ArtworkCommentReference>();
    }

    public Task<bool> RemoveCommentAsync(long CommentId)
    {
        try
        {
            var result = _artworkCommentDbSet.Find(CommentId);
            _artworkCommentDbSet.Remove(result);
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
