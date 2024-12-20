using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
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
    private readonly DbSet<ChatGroup> _chatGroupContext;
    private readonly DbSet<ChatGroupMember> _chatGroupMemberContext;
    private readonly DbSet<ChatMessage> _chatMessageContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserFriendRequest> _userFriendRequestContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;
    private readonly DbSet<UserProfile> _userProfile;

    public SM31Repository(AppDbContext context, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = context;
        _userFriendRequestContext = context.Set<UserFriendRequest>();
        _userFriendContext = context.Set<UserFriend>();
        _userManager = userManager;
        _userProfile = context.Set<UserProfile>();
        _chatMessageContext = context.Set<ChatMessage>();
        _chatGroupMemberContext = context.Set<ChatGroupMember>();
        _chatGroupContext = context.Set<ChatGroup>();
    }

    public async Task<bool> IsAlreadyFriendAsync(long userId, long friendId, CancellationToken ct)
    {
        return await _userFriendContext.AnyAsync(
            x =>
                (x.UserId == userId && x.FriendId == friendId)
                || (x.UserId == friendId && x.FriendId == userId),
            ct
        );
    }

    public async Task<bool> UnfriendAsync(IEnumerable<UserFriend> userFriends, CancellationToken ct)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        await executionStrategy.ExecuteAsync(
            async () => await InternalUnFriendPostAsync(userFriends, ct, result)
        );
        return result.Value;
    }

    public async Task<bool> IsUserExistAsync(long friendId, CancellationToken token)
    {
        var user = await _userManager.Value.FindByIdAsync(friendId.ToString());
        var profile = await _userProfile.FindAsync(friendId, token);
        return user != null || profile != null;
    }

    public async Task<bool> HasFriendRequestAsync(long userId, long friendId, CancellationToken ct)
    {
        return await _userFriendRequestContext.AnyAsync(
            x => x.CreatedBy == userId && x.FriendId == friendId,
            ct
        );
    }

    private async Task InternalUnFriendPostAsync(
        IEnumerable<UserFriend> friendRequest,
        CancellationToken token,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;
        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(_dbContext, token);
            var userFriendPairs = friendRequest.First();
            await _chatMessageContext
                .Where(x =>
                    (
                        x.CreatedBy == userFriendPairs.UserId
                        && x.CreatedBy == userFriendPairs.FriendId
                    )
                )
                .ExecuteDeleteAsync(token);

            await _chatGroupMemberContext
                .Where(x =>
                    x.MemberId == userFriendPairs.UserId || x.MemberId == userFriendPairs.FriendId
                )
                .ExecuteDeleteAsync(token);

            await _chatGroupContext
                .Where(x =>
                    x.GroupName.Contains(userFriendPairs.UserId.ToString())
                    && x.GroupName.Contains(userFriendPairs.FriendId.ToString())
                )
                .ExecuteDeleteAsync(token);

            await _userFriendContext
                .Where(x =>
                    (x.UserId == userFriendPairs.UserId && x.FriendId == userFriendPairs.FriendId)
                    || (
                        x.UserId == userFriendPairs.FriendId && x.FriendId == userFriendPairs.UserId
                    )
                )
                .ExecuteDeleteAsync(token);
            await _userFriendRequestContext
                .Where(x =>
                    (
                        x.CreatedBy == userFriendPairs.UserId
                        && x.FriendId == userFriendPairs.FriendId
                    )
                    || (
                        x.CreatedBy == userFriendPairs.FriendId
                        && x.FriendId == userFriendPairs.UserId
                    )
                )
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
