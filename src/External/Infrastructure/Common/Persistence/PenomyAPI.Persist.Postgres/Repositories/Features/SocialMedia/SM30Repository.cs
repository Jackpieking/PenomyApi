using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM30Repository : ISM30Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;
    private readonly DbSet<UserFriendRequest> _userPostContext;

    public SM30Repository(AppDbContext context, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = context;
        _userPostContext = context.Set<UserFriendRequest>();
        _userFriendContext = context.Set<UserFriend>();
        _userManager = userManager;
    }

    public async Task<bool> SendFriendRequest(UserFriendRequest userFriendRequest, CancellationToken token)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(async () =>
            await InternalSendFriendPostAsync(
                userFriendRequest,
                token,
                result
            ));
        return result.Value;
    }

    public async Task<bool> IsUserExistAsync(long friendId, CancellationToken token)
    {
        var user = await _userManager.Value.FindByIdAsync(friendId.ToString());
        return user != null;
    }

    public async Task<bool> IsAlreadyFriendAsync(long userId, long friendId, CancellationToken token)
    {
        return await _dbContext.Set<UserFriend>().AnyAsync(x => x.UserId == userId && x.FriendId == friendId, token);
    }

    private async Task InternalSendFriendPostAsync(UserFriendRequest friendRequest,
        CancellationToken token, Result<bool> result)
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                token
            );
            await _userPostContext.AddAsync(friendRequest, token);
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
