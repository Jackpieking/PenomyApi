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
            var result = _artworkCommentDbSet.Find(CommentId);
            var ChildComment = _commentParentChildDbSet
                .Where(x => x.ChildCommentId == CommentId)
                .FirstOrDefault();

            if (ChildComment != null)
            {
                await _artworkCommentDbSet
                    .Where(x => x.Id == ChildComment.ParentCommentId)
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(x => x.TotalChildComments, x => x.TotalChildComments - 1)
                    );
                _commentParentChildDbSet.Remove(ChildComment);
            }

            _artworkCommentDbSet.Remove(result);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
