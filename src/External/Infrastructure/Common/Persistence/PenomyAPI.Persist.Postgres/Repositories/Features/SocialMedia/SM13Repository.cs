using System;
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

public class SM13Repository : ISM13Repository
{
    private readonly DbSet<UserPostAttachedMedia> _attachedMediaContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserPostLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<UserPost> _userPostContext;
    private readonly DbSet<UserPostReport> _userPostReportContext;
    private readonly DbSet<UserProfile> _userProfileContext;

    public SM13Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userPostReportContext = _dbContext.Set<UserPostReport>();
        _userPostContext = _dbContext.Set<UserPost>();
        _attachedMediaContext = _dbContext.Set<UserPostAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<UserPostLikeStatistic>();
        _userProfileContext = _dbContext.Set<UserProfile>();
    }

    public async Task<bool> IsUserPostExistedAsync(long id, CancellationToken cancellationToken)
    {
        return await _userPostContext.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<UserPost> GetUserPostByIdAsync(long id, CancellationToken cancellationToken)
    {
        var userPost = await _userPostContext.Where(x => x.Id == id).Select(x => new UserPost
        {
            Content = x.Content,
            AllowComment = x.AllowComment,
            PublicLevel = x.PublicLevel,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Id = x.Id
        }).FirstOrDefaultAsync(cancellationToken);
        return userPost;
    }

    public async Task<bool> UpdateUserPostAsync(UserPost updatePost, bool isImageUpdate,
        IEnumerable<UserPostAttachedMedia> attachedMediae,
        CancellationToken token = default)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            async () =>
                await InternalUpdatePostAsync(
                    updatePost,
                    isImageUpdate,
                    attachedMediae,
                    token,
                    result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdatePostAsync(
        UserPost updateDetail,
        bool isMediaUpdated,
        IEnumerable<UserPostAttachedMedia> attachedMediae,
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
            await _userPostContext.Where(x => x.Id == updateDetail.Id).ExecuteUpdateAsync(
                updatePost => updatePost.SetProperty(post => post.Content, updateDetail.Content)
                    .SetProperty(post => post.AllowComment, updateDetail.AllowComment)
                    .SetProperty(post => post.PublicLevel, updateDetail.PublicLevel)
                    .SetProperty(post => post.UpdatedAt, updateDetail.UpdatedAt)
            );
            if (isMediaUpdated)
            {
                await _attachedMediaContext.Where(x => x.PostId == updateDetail.Id)
                    .ExecuteDeleteAsync(cancellationToken);
                await _attachedMediaContext.AddRangeAsync(attachedMediae, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
}
