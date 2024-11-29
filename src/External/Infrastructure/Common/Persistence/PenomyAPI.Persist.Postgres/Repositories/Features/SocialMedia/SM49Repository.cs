using System;
using System.Linq;
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

public class SM49Repository : ISM49Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserFriendRequest> _userFriendRequestContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public SM49Repository(AppDbContext context, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = context;
        _userFriendRequestContext = context.Set<UserFriendRequest>();
        _userFriendContext = context.Set<UserFriend>();
        _userManager = userManager;
    }

    public async Task<bool> IsFriendRequestExists(long userId, long friendId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private async Task InternalAcceptFriendAsync(UserFriend friendRequest,
        CancellationToken token, Result<bool> result)
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                token
            );

            await _userFriendContext.AddAsync(friendRequest, token);
            await _userFriendRequestContext
                .Where(x => x.CreatedBy == friendRequest.UserId && x.FriendId == friendRequest.FriendId)
                .ExecuteDeleteAsync(token);
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
