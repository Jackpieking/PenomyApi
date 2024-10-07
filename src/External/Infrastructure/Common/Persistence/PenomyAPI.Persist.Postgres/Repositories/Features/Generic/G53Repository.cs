using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G53Repository : IG53Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;

    public G53Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
    }

    public async Task<bool> EditCommentAsync(long CommentId, string NewComment)
    {

        var comment = await _artworkCommentDbSet.FindAsync(CommentId);
        if (comment == null) return false;

        comment.Content = NewComment;
        await _dbContext.SaveChangesAsync();
        return true;

    }
}
