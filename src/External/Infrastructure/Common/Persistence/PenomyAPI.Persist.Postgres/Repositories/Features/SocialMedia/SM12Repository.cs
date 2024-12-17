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
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM12Repository : ISM12Repository
{
    private readonly DbSet<UserPostAttachedMedia> _attachedMediaContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriendRequest> _friendRequests;
    private readonly DbSet<UserPostLikeStatistic> _likeStatisticsContext;
    private readonly DbSet<UserPost> _userPostContext;
    private readonly DbSet<UserPostReport> _userPostReportContext;
    private readonly DbSet<UserProfile> _userProfileContext;

    public SM12Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userPostReportContext = _dbContext.Set<UserPostReport>();
        _userPostContext = _dbContext.Set<UserPost>();
        _attachedMediaContext = _dbContext.Set<UserPostAttachedMedia>();
        _likeStatisticsContext = _dbContext.Set<UserPostLikeStatistic>();
        _userProfileContext = _dbContext.Set<UserProfile>();
        _friendRequests = _dbContext.Set<UserFriendRequest>();
    }

    public async Task<bool> CreateUserPostAsync(
        UserPost createdPost,
        IEnumerable<UserPostAttachedMedia> postAttachedMediae,
        UserPostLikeStatistic postLikeStatistic,
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

    public async Task<List<UserFriendRequest>> GetUserFriendRequestsAsync(long userId,
        CancellationToken token = default)
    {
        return await _friendRequests.Where(x => x.FriendId == userId && x.RequestStatus == RequestStatus.Pending)
            .Select(x => new UserFriendRequest
            {
                FriendId = x.FriendId,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                RequestStatus = x.RequestStatus
            }).ToListAsync(token);
    }

    public Task<UserProfile> GetUserProfileAsync(long userId, CancellationToken token = default)
    {
        return _userProfileContext
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UserId == userId, token);
    }

    private async Task InternalCreateUserPostAsync(
        UserPost createdPost,
        IEnumerable<UserPostAttachedMedia> postAttachedMediae,
        UserPostLikeStatistic postLikeStatistic,
        CancellationToken token,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            await _userPostContext.AddAsync(createdPost, token);
            await _attachedMediaContext.AddRangeAsync(postAttachedMediae, token);
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
