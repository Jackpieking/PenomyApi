using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM24Repository : ISM24Repository
{
    private readonly DbSet<UserPostComment> _userPostCommentDbSet;
    private readonly DbContext _dbContext;

    public SM24Repository(DbContext dbContext)
    {
        _userPostCommentDbSet = dbContext.Set<UserPostComment>();
        _dbContext = dbContext;
    }

    public async Task<long> CreatePostCommentsAsync(
        UserPostComment comment,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await _userPostCommentDbSet.AddAsync(comment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return comment.Id;
        }
        catch
        {
            return -1;
        }
    }
}
