using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM32Repository : ISM32Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserFriendRequest> _userFriendRequestContext;
    private readonly DbSet<UserProfile> _userProfileContext;
    private readonly DbSet<ChatGroup> _chatGroupContext;

    public SM32Repository(AppDbContext context)
    {
        _dbContext = context;
        _userFriendContext = context.Set<UserFriend>();
        _userProfileContext = context.Set<UserProfile>();
        _userFriendRequestContext = context.Set<UserFriendRequest>();
        _chatGroupContext = context.Set<ChatGroup>();
    }

    public async Task<IEnumerable<long>> GetAllUserFriendRequestAsync(
        long userId,
        CancellationToken token
    )
    {
        var result = await _userFriendRequestContext
            .Where(x => x.CreatedBy == userId && x.RequestStatus == RequestStatus.Pending)
            .Select(x => x.FriendId)
            .ToListAsync(token);

        return result.Distinct();
    }

    public async Task<IEnumerable<long>> GetUserFriendRequestAsync(
        long userId,
        CancellationToken token
    )
    {
        return await _userFriendRequestContext
            .Where(x => x.FriendId == userId && x.RequestStatus == RequestStatus.Pending)
            .Select(x => x.CreatedBy)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync(
        long userId,
        CancellationToken token
    )
    {
        return await _userProfileContext
            .Where(x => x.UserId != userId)
            .Select(x => new UserProfile
            {
                UserId = x.UserId,
                NickName = x.NickName,
                AvatarUrl = x.AvatarUrl,
                Gender = x.Gender,
                AboutMe = x.AboutMe,
            })
            .ToListAsync(token);
    }

    public async Task<IEnumerable<UserProfile>> GetAllUserFriendsAsync(
        long userId,
        long friendId,
        CancellationToken token
    )
    {
        var friendIds = await _userFriendContext
            .Where(x => x.UserId == userId)
            .Select(x => x.FriendId)
            .ToListAsync(token);
        var friendIds1 = await _userFriendContext
            .Where(x => x.FriendId == userId)
            .Select(x => x.UserId)
            .ToListAsync(token);
        var listFriends = friendIds.Union(friendIds1).Where(x => x != userId).ToList();

        var friendList = await _userProfileContext
            .Where(x => listFriends.Contains(x.UserId))
            .Select(x => new UserProfile
            {
                UserId = x.UserId,
                NickName = x.NickName,
                AvatarUrl = x.AvatarUrl,
                Gender = x.Gender,
                AboutMe = x.AboutMe,
            })
            .ToListAsync(token);

        foreach (var friend in friendList)
        {
            var myChatGroups = await _chatGroupContext
                .Where(x =>
                    x.GroupName.Contains(userId.ToString())
                    && x.GroupName.Contains(friend.UserId.ToString())
                )
                .FirstOrDefaultAsync(token);

            friend.JoinedChatGroupMembers.Append(
                new ChatGroupMember { ChatGroupId = myChatGroups.Id }
            );
        }

        return friendList;
    }
}
