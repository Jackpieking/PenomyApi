using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM31Repository : ISM31Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserFriendRequest> _userFriendRequestContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public SM31Repository(AppDbContext context, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = context;
        _userFriendRequestContext = context.Set<UserFriendRequest>();
        _userFriendContext = context.Set<UserFriend>();
        _userManager = userManager;
    }

    public async Task<bool> IsAlreadyFriendAsync(long userId, long friendId, CancellationToken ct)
    {
        return await _userFriendContext.AnyAsync(x => x.UserId == userId && x.FriendId == friendId, ct);
    }

    public async Task<bool> UnfriendAsync(UserFriend userFriend, CancellationToken ct)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(async () =>
            await InternalUnFriendPostAsync(
                userFriend,
                ct,
                result
            ));
        return result.Value;
    }

    public async Task<bool> IsUserExistAsync(long friendId, CancellationToken token)
    {
        var user = await _userManager.Value.FindByIdAsync(friendId.ToString());
        return user != null;
    }

    private async Task InternalUnFriendPostAsync(UserFriend friendRequest,
        CancellationToken token, Result<bool> result)
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                token
            );
            await _userFriendContext
                .Where(x => x.UserId == friendRequest.UserId && x.FriendId == friendRequest.FriendId)
                .ExecuteDeleteAsync(token);
            await _userFriendRequestContext
                .Where(x => (x.CreatedBy == friendRequest.UserId && x.FriendId == friendRequest.FriendId &&
                             x.RequestStatus == RequestStatus.Accepted) ||
                            (x.CreatedBy == friendRequest.FriendId && x.FriendId == friendRequest.UserId &&
                             x.RequestStatus == RequestStatus.Accepted))
                .ExecuteDeleteAsync(token);
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
            result.Value = true;
        }
        catch (Exception e)
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