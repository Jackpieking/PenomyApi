using System;
using System.Collections.Generic;
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

public class SM34Repository : ISM34Repository
{
    private readonly DbSet<GroupPostAttachedMedia> _attachedMediaContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<GroupPostLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<GroupPost> _groupPostContext;
    private readonly DbSet<GroupPostReport> _groupPostReportContext;
    private readonly DbSet<UserProfile> _userProfileContext;

    public SM34Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _groupPostReportContext = _dbContext.Set<GroupPostReport>();
        _groupPostContext = _dbContext.Set<GroupPost>();
        _attachedMediaContext = _dbContext.Set<GroupPostAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<GroupPostLikeStatistic>();
        _userProfileContext = _dbContext.Set<UserProfile>();
    }

    public async Task<bool> CreateGroupPostAsync(
        GroupPost createdPost,
        IEnumerable<GroupPostAttachedMedia> postAttachedMediae,
        GroupPostLikeStatistic postLikeStatistic,
        CancellationToken token = default
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(
            async () =>
                await InternalCreateUserPostAsync(
                    createdPost,
                    postAttachedMediae,
                    postLikeStatistic,
                    token,
                    result
                )
        );
        return result.Value;
    }

    public Task<UserProfile> GetUserProfileAsync(long userId, CancellationToken token = default)
    {
        return _userProfileContext
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UserId == userId, token);
    }

    private async Task InternalCreateUserPostAsync(
        GroupPost createdPost,
        IEnumerable<GroupPostAttachedMedia> postAttachedMedia,
        GroupPostLikeStatistic postLikeStatistic,
        CancellationToken token,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _groupPostContext.AddAsync(createdPost, token);
            await _attachedMediaContext.AddRangeAsync(postAttachedMedia, token);
            // await _likeStatisticsContext.AddAsync(postLikeStatistic, token);
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = true;
        }
        catch (Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(token);
                await transaction.DisposeAsync();
            }

            result.Value = false;
        }
    }
}
