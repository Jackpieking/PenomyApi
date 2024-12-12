using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM25Repository : ISM25Repository
{
    private readonly DbSet<UserPostComment> _userPostCommentDbSet;
    private readonly DbContext _dbContext;

    public SM25Repository(DbContext dbContext)
    {
        _userPostCommentDbSet = dbContext.Set<UserPostComment>();
        _dbContext = dbContext;
    }

    public async Task<bool> UpdatePostCommentsAsync(
        long commentId,
        string newComment,
        long userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await _userPostCommentDbSet
                .Where(c => c.Id == commentId && c.CreatedBy == userId)
                .ExecuteUpdateAsync(
                    c => c.SetProperty(c => c.Content, newComment),
                    cancellationToken
                );
            return true;
        }
        catch
        {
            return false;
        }
    }
}
