using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM17Repository : ISM17Repository
{
    // Return string 11 for like success, 01 for unlike success
    // Return string 10 for like failure, 00 for unlike failure
    private readonly DbSet<UserPost> _userPostContext;
    private readonly DbSet<GroupPost> _groupPostContext;
    private readonly DbSet<UserLikeGroupPost> _userLikeGroupPostContext;
    private readonly DbSet<UserLikeUserPost> _userLikeUserPostContext;
    private readonly DbContext _dbContext;

    public SM17Repository(AppDbContext context)
    {
        _userPostContext = context.Set<UserPost>();
        _groupPostContext = context.Set<GroupPost>();
        _userLikeGroupPostContext = context.Set<UserLikeGroupPost>();
        _userLikeUserPostContext = context.Set<UserLikeUserPost>();
        _dbContext = context;
    }

    public async Task<string> LikeUnlikePostAsync(
        long userId,
        long postId,
        bool isGroupPost,
        CancellationToken token
    )
    {
        var result = new Result<string>("");
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        if (isGroupPost)
        {
            var userLike = _userLikeGroupPostContext.FirstOrDefault(p =>
                p.UserId == userId && p.PostId == postId
            );
            if (userLike == null)
                await executionStrategy.ExecuteAsync(
                    operation: async () => await LikeGroupPostAsync(userId, postId, result, token)
                );
            else
                await executionStrategy.ExecuteAsync(
                    operation: async () => await UnlikeGroupPostAsync(userId, postId, result, token)
                );
        }
        else
        {
            var userLike = _userLikeUserPostContext.FirstOrDefault(p =>
                p.UserId == userId && p.PostId == postId
            );
            if (userLike == null)
                await executionStrategy.ExecuteAsync(
                    operation: async () => await LikeUserPostAsync(userId, postId, result, token)
                );
            else
                await executionStrategy.ExecuteAsync(
                    operation: async () => await UnlikeUserPostAsync(userId, postId, result, token)
                );
        }

        return result.Value;
    }

    public async Task LikeGroupPostAsync(
        long userId,
        long postId,
        Result<string> result,
        CancellationToken token
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _userLikeGroupPostContext.AddAsync(
                new UserLikeGroupPost
                {
                    UserId = userId,
                    PostId = postId,
                    LikedAt = DateTime.UtcNow,
                    ValueId = 1,
                }
            );
            await _groupPostContext
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.TotalLikes, p => p.TotalLikes + 1),
                    token
                );
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = "11";
        }
        catch
        {
            result.Value = "10";
        }
    }

    public async Task LikeUserPostAsync(
        long userId,
        long postId,
        Result<string> result,
        CancellationToken token
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _userLikeUserPostContext.AddAsync(
                new UserLikeUserPost
                {
                    UserId = userId,
                    PostId = postId,
                    LikedAt = DateTime.UtcNow,
                    ValueId = 1,
                }
            );

            await _userPostContext
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.TotalLikes, p => p.TotalLikes + 1),
                    token
                );

            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = "11";
        }
        catch
        {
            result.Value = "10";
        }
    }

    public async Task UnlikeGroupPostAsync(
        long userId,
        long postId,
        Result<string> result,
        CancellationToken token
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _userLikeGroupPostContext
                .Where(p => p.UserId == userId && p.PostId == postId)
                .ExecuteDeleteAsync(token);
            await _groupPostContext
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.TotalLikes, p => p.TotalLikes - 1),
                    token
                );

            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = "01";
        }
        catch
        {
            result.Value = "00";
        }
    }

    public async Task UnlikeUserPostAsync(
        long userId,
        long postId,
        Result<string> result,
        CancellationToken token
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _userLikeUserPostContext
                .Where(p => p.UserId == userId && p.PostId == postId)
                .ExecuteDeleteAsync(token);

            await _userPostContext
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(p => p.TotalLikes, p => p.TotalLikes - 1),
                    token
                );

            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = "01";
        }
        catch
        {
            result.Value = "00";
        }
    }
}
