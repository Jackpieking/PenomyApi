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
    private readonly DbSet<UserLikeUserPost> _likeUserPostContext;
    private readonly DbSet<UserPost> _userPostContext;

    public SM15Repository(AppDbContext context)
    {
        _likeUserPostContext = context.Set<UserLikeUserPost>();
        _userPostContext = context.Set<UserPost>();
    }

    public async Task<List<(bool, long)>> IsLikePostAsync(long userId, List<string> postIds, CancellationToken token)
    {
        var postIdsLong = postIds.Select(long.Parse).ToList();

        var likedPosts = await _likeUserPostContext
            .Where(like => like.UserId == userId && postIdsLong.Contains(like.PostId))
            .Select(like => like.PostId)
            .ToListAsync(token);

        var result = postIdsLong
            .Select(postId => (likedPosts.Contains(postId), postId))
            .ToList();

        return result;
    }

    public async Task<List<UserPost>> GetPersonalPostsAsync(long userId, CancellationToken token)
    {
        return await _userPostContext
            .Where(x => x.CreatedBy == userId)
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
                    AvatarUrl = x.Creator.AvatarUrl
                },
                AttachedMedias = x.AttachedMedias.Select(y => new UserPostAttachedMedia
                {
                    FileName = y.FileName,
                    MediaType = y.MediaType,
                    StorageUrl = y.StorageUrl,
                    UploadOrder = y.UploadOrder
                })
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(token);
    }
}
