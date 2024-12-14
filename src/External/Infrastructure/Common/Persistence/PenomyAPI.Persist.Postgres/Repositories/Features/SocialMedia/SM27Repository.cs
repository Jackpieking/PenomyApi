using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM27Repository : ISM27Repository
{
    private readonly DbSet<UserPostComment> _userPostCommentDbSet;
    private readonly DbSet<UserPost> _userPostDbSet;
    private readonly DbContext _dbContext;

    public SM27Repository(DbContext dbContext)
    {
        _userPostCommentDbSet = dbContext.Set<UserPostComment>();
        _userPostDbSet = dbContext.Set<UserPost>();
        _dbContext = dbContext;
    }

    public async Task<bool> CheckPostOnwerAsync(
        long postId,
        long userId,
        CancellationToken cancellationToken
    )
    {
        return await _userPostDbSet
            .Where(p => p.Id == postId && p.CreatedBy == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> TakeDownPostCommentsAsync(
        long commentId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await _userPostCommentDbSet
                .Where(c => c.Id == commentId)
                .ExecuteUpdateAsync(c => c.SetProperty(c => c.IsRemoved, true), cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
