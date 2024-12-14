using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
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
    private readonly DbSet<UserPostComment> _userPostCommentContext;
    private readonly DbSet<UserPost> _userPostContext;
    private readonly DbSet<UserPostReport> _userPostReportContext;
    private readonly DbSet<UserProfile> _userProfileContext;

    public SM14Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userPostContext = _dbContext.Set<UserPost>();
        _attachedMediaContext = _dbContext.Set<UserPostAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<UserPostLikeStatistic>();
        _userProfileContext = _dbContext.Set<UserProfile>();
        _userPostCommentContext = _dbContext.Set<UserPostComment>();
        _userPostReportContext = _dbContext.Set<UserPostReport>();
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
        CancellationToken cancellationToken
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

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

            await _attachedMediaContext
                .Where(media => media.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);

            await _userPostCommentContext
                .Where(comment => comment.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _userPostReportContext
                .Where(rp => rp.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
            await _likeStatisticsContext
                .Where(l => l.PostId == postId)
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
}
