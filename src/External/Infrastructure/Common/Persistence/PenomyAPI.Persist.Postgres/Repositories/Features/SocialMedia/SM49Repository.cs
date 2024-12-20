using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM49Repository : ISM49Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserFriendRequest> _userFriendRequestContext;

    public SM49Repository(AppDbContext context)
    {
        _dbContext = context;
        _userFriendRequestContext = context.Set<UserFriendRequest>();
        _userFriendContext = context.Set<UserFriend>();
    }

    public async Task<bool> IsFriendRequestExistsAsync(long userId, long friendId, CancellationToken token)
    {
        return await _userFriendRequestContext.AnyAsync(
            x => x.CreatedBy == userId && x.FriendId == friendId && x.RequestStatus == RequestStatus.Pending,
            token);
    }

    public async Task<bool> IsUserFriendExistsAsync(long userId, long friendId, CancellationToken token)
    {
        return await _userFriendContext.AnyAsync(x => x.UserId == userId && x.FriendId == friendId, token);
    }

    public async Task<bool> AcceptFriendRequestAsync(IEnumerable<UserFriend> userFriend, CancellationToken token)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(async () =>
            await InternalAcceptFriendAsync(
                userFriend.ToList(),
                token,
                result
            ));
        return result.Value;
    }

    private async Task InternalAcceptFriendAsync(
        List<UserFriend> friendRequests,
        CancellationToken token,
        Result<bool> result)
    {
        if (friendRequests == null || friendRequests.Count == 0)
        {
            result.Value = false;
            return;
        }

        IDbContextTransaction transaction = null;
        try
        {
            // Start the transaction
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);

            // Add friend relationships
            await _userFriendContext.AddRangeAsync(friendRequests, token);

            // Extract UserId and FriendId pairs from the friendRequests
            var userFriendPairs = friendRequests
                .Select(fr => new { fr.UserId, fr.FriendId })
                .ToList();

            // Update the friend request status
            await _userFriendRequestContext
                .Where(x =>
                    x.CreatedBy == userFriendPairs.First().UserId && x.FriendId == userFriendPairs.First().FriendId)
                .ExecuteUpdateAsync(
                    x => x.SetProperty(y => y.RequestStatus, RequestStatus.Accepted),
                    token
                );

            // Save changes and commit transaction
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);

            result.Value = true;
        }
        catch (Exception)
        {
            // Rollback and clean up the transaction
            if (transaction != null)
            {
                await transaction.RollbackAsync(token);
                await transaction.DisposeAsync();
            }

            result.Value = false;
        }
        finally
        {
            // Ensure transaction is properly disposed in any scenario
            if (transaction != null) await transaction.DisposeAsync();
        }
    }
}
