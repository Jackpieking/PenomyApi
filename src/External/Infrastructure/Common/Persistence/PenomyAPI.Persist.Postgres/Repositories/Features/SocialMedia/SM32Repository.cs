using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Data.DbContexts;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM32Repository : ISM32Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserProfile> _userProfileContext;

    public SM32Repository(AppDbContext context)
    {
        _dbContext = context;
        _userFriendContext = context.Set<UserFriend>();
        _userProfileContext = context.Set<UserProfile>();
    }


    public async Task<IEnumerable<long>> GetAllUserFriendsAsync(long userId, CancellationToken token)
    {
        return await _userFriendContext.Where(x => x.UserId == userId).Select(x =>
            x.FriendId
        ).ToListAsync(token);
    }

    public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync(IEnumerable<long> userIds,
        CancellationToken token)
    {
        return await _userProfileContext.Where(x => userIds.Contains(x.UserId)).Select(x => new UserProfile
        {
            UserId = x.UserId,
            NickName = x.NickName,
            AvatarUrl = x.AvatarUrl,
            Gender = x.Gender,
            AboutMe = x.AboutMe
        }).ToListAsync(token);
    }
}