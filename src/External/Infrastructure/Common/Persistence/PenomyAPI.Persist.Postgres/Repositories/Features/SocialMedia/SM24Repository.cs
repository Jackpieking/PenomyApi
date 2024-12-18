using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM24Repository : ISM24Repository
{
    private readonly DbSet<GroupPost> _groupPostDbSet;
    private readonly DbSet<UserPost> _userPostDbSet;
    private readonly DbSet<UserPostComment> _userPostCommentDbSet;
    private readonly DbSet<GroupPostComment> _groupPostCommentDbSet;
    private readonly DbContext _dbContext;

    public SM24Repository(DbContext dbContext)
    {
        _groupPostDbSet = dbContext.Set<GroupPost>();
        _userPostDbSet = dbContext.Set<UserPost>();
        _userPostCommentDbSet = dbContext.Set<UserPostComment>();
        _groupPostCommentDbSet = dbContext.Set<GroupPostComment>();
        _dbContext = dbContext;
    }

    public async Task<long> CreateUserPostCommentsAsync(
        UserPostComment comment,
        CancellationToken cancellationToken
    )
    {
        Result<long> result = new Result<long>();
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            async () => await CreateUserPostComment(comment, result, cancellationToken)
        );

        return result.Value;
    }

    public async Task<long> CreateGroupPostCommentsAsync(
        GroupPostComment comment,
        CancellationToken cancellationToken
    )
    {
        Result<long> result = new Result<long>();
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            async () => await CreateGroupPostComment(comment, result, cancellationToken)
        );

        return result.Value;
    }

    private async Task CreateGroupPostComment(
        GroupPostComment comment,
        Result<long> result,
        CancellationToken cancellationToken
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            await _groupPostCommentDbSet.AddAsync(comment, cancellationToken);
            await _groupPostDbSet
                .Where(p => p.Id == comment.PostId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.UpdatedAt, p => DateTime.UtcNow),
                    cancellationToken
                );
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            result.Value = comment.Id;
        }
        catch
        {
            result.Value = -1;
        }
    }

    private async Task CreateUserPostComment(
        UserPostComment comment,
        Result<long> result,
        CancellationToken cancellationToken
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            await _userPostCommentDbSet.AddAsync(comment, cancellationToken);
            await _userPostDbSet
                .Where(p => p.Id == comment.PostId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.UpdatedAt, p => DateTime.UtcNow),
                    cancellationToken
                );
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            result.Value = comment.Id;
        }
        catch
        {
            result.Value = -1;
        }
    }
}
