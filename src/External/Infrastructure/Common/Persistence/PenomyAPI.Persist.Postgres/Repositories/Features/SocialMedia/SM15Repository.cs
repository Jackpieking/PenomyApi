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

public class SM15Repository : ISM15Repository
{
    private readonly DbSet<UserFriend> _userFriendContext;
    private readonly DbSet<UserPost> _userPostContext;

    public SM15Repository(AppDbContext context)
    {
        _userPostContext = context.Set<UserPost>();
        _userFriendContext = context.Set<UserFriend>();
    }

    public async Task<List<UserPost>> GetPersonalPostsAsync(long userId, CancellationToken token)
    {
        return await _userPostContext
            .Where(x => x.PublicLevel == UserPostPublicLevel.Everyone)
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new UserPost
            {
                Id = x.Id,
                Content = x.Content,
                CreatedBy = x.CreatedBy,
                AllowComment = x.AllowComment,
                PublicLevel = x.PublicLevel,
                TotalLikes = x.TotalLikes,
                CreatedAt = x.CreatedAt,
                Creator = new UserProfile
                {
                    UserId = x.Creator.UserId,
                    NickName = x.Creator.NickName,
                    AvatarUrl = x.Creator.AvatarUrl,
                },
                AttachedMedias = x.AttachedMedias.Select(y => new UserPostAttachedMedia
                {
                    FileName = y.FileName,
                    MediaType = y.MediaType,
                    StorageUrl = y.StorageUrl,
                    UploadOrder = y.UploadOrder,
                }),
                UserLikes = x.UserLikes.ToList(),
            })
            .ToListAsync(token);
    }

    public async Task<List<UserPost>> GetUserPostsAsync(long userId, CancellationToken token)
    {
        var friendIs = await _userFriendContext
            .Where(x => x.UserId == userId)
            .Select(x => x.FriendId)
            .ToListAsync(token);
        friendIs.AddRange(
            await _userFriendContext
                .Where(x => x.FriendId == userId)
                .Select(x => x.UserId)
                .ToListAsync(token)
        );

        return await GetFriendsPostsAsync(friendIs.Distinct().ToList(), token);
    }

    private async Task<List<UserPost>> GetFriendsPostsAsync(List<long> ids, CancellationToken token)
    {
        if (ids == null)
            return [];
        return await _userPostContext
            .Where(x =>
                ids.Contains(x.CreatedBy) || x.PublicLevel == UserPostPublicLevel.OnlyFriend
            )
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new UserPost
            {
                Id = x.Id,
                Content = x.Content,
                CreatedBy = x.CreatedBy,
                AllowComment = x.AllowComment,
                PublicLevel = x.PublicLevel,
                TotalLikes = x.TotalLikes,
                CreatedAt = x.CreatedAt,
                Creator = new UserProfile
                {
                    UserId = x.Creator.UserId,
                    NickName = x.Creator.NickName,
                    AvatarUrl = x.Creator.AvatarUrl,
                },
                AttachedMedias = x.AttachedMedias.Select(y => new UserPostAttachedMedia
                {
                    FileName = y.FileName,
                    MediaType = y.MediaType,
                    StorageUrl = y.StorageUrl,
                    UploadOrder = y.UploadOrder,
                }),
                UserLikes = x.UserLikes.ToList(),
            })
            .ToListAsync(token);
    }
}
