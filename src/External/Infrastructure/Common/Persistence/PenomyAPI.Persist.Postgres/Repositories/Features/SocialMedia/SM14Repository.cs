using System.Collections.Generic;
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

public class SM14Repository : ISM14Repository
{
    private readonly DbSet<UserPostAttachedMedia> _attachedMediaContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserPostLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<UserLikeUserPost> _userLikeUserPostContext;
    private readonly DbSet<UserPostComment> _userPostCommentContext;
    private readonly DbSet<UserPost> _userPostContext;
    private readonly DbSet<UserPostReport> _userPostReportContext;

    // group post
    private readonly DbSet<GroupPostAttachedMedia> _groupAttachedMediaContext;
    private readonly DbSet<GroupPostLikeStatistic> _groupLikeStatisticsContext;
    private readonly DbSet<UserLikeGroupPost> _userLikegroupPostContext;
    private readonly DbSet<GroupPostComment> _groupPostCommentContext;
    private readonly DbSet<GroupPost> _groupPostContext;
    private readonly DbSet<GroupPostReport> _groupPostReportContext;

    public SM14Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userPostContext = _dbContext.Set<UserPost>();
        _attachedMediaContext = _dbContext.Set<UserPostAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<UserPostLikeStatistic>();
        _userPostCommentContext = _dbContext.Set<UserPostComment>();
        _userPostReportContext = _dbContext.Set<UserPostReport>();
        _userLikeUserPostContext = _dbContext.Set<UserLikeUserPost>();

        _groupPostContext = _dbContext.Set<GroupPost>();
        _groupLikeStatisticsContext = _dbContext.Set<GroupPostLikeStatistic>();
        _userLikegroupPostContext = _dbContext.Set<UserLikeGroupPost>();
        _groupPostCommentContext = _dbContext.Set<GroupPostComment>();
        _groupPostReportContext = _dbContext.Set<GroupPostReport>();
        _groupAttachedMediaContext = _dbContext.Set<GroupPostAttachedMedia>();
    }

    public async Task<List<long>> GetAttachedFileIdAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        return await _userPostContext
            .Where(x => x.CreatedBy == userId)
            .SelectMany(x => x.AttachedMedias.Select(media => media.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<long>> GetGroupPostAttachedFileIdAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        return await _groupPostContext
            .Where(x => x.CreatedBy == userId)
            .SelectMany(x => x.AttachedMedias.Select(media => media.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsExistUserPostAsync(
        long id,
        long userId,
        CancellationToken cancellationToken
    )
    {
        return await _userPostContext.AnyAsync(
            x => x.Id == id && x.CreatedBy == userId,
            cancellationToken
        );
    }

    public async Task<bool> RemoveUserPostAsync(
        long id,
        long userId,
        bool isGroupPost,
        CancellationToken cancellationToken
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        if (isGroupPost)
        {
            await executionStrategy.ExecuteAsync(
                async () =>
                    await InternalRemoveGroupPostByIdAsync(id, userId, cancellationToken, result)
            );
        }
        await executionStrategy.ExecuteAsync(
            async () => await InternalRemoveUserPostByIdAsync(id, userId, cancellationToken, result)
        );

        return result.Value;
    }

    private async Task InternalRemoveUserPostByIdAsync(
        long postId,
        long userId,
        CancellationToken cancellationToken,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _userLikeUserPostContext
                .Where(l => l.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _likeStatisticsContext
                .Where(l => l.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _attachedMediaContext
                .Where(media => media.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);

            await _userPostCommentContext
                .Where(comment => comment.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _userPostReportContext
                .Where(rp => rp.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);

            // Set the temporarily removed flag first.
            await _userPostContext
                .Where(updatedArtwork =>
                    updatedArtwork.Id == postId && updatedArtwork.CreatedBy == userId
                )
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }

    private async Task InternalRemoveGroupPostByIdAsync(
        long postId,
        long userId,
        CancellationToken cancellationToken,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _userLikegroupPostContext
                .Where(l => l.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _groupLikeStatisticsContext
                .Where(l => l.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _groupAttachedMediaContext
                .Where(media => media.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);

            await _groupPostCommentContext
                .Where(comment => comment.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _groupPostReportContext
                .Where(rp => rp.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);

            // Set the temporarily removed flag first.
            await _groupPostContext
                .Where(updatedArtwork =>
                    updatedArtwork.Id == postId && updatedArtwork.CreatedBy == userId
                )
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
}
