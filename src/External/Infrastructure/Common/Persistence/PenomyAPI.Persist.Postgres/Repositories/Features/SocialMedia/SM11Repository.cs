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

public class SM11Repository : ISM11Repository
{
    private readonly DbSet<GroupPost> _groupPostContext;

    public SM11Repository(AppDbContext context)
    {
        _groupPostContext = context.Set<GroupPost>();
    }

    public async Task<List<GroupPost>> GetGroupPostsAsync(
        long userId,
        long groupId,
        CancellationToken token
    )
    {
        return await _groupPostContext
            .Where(x => x.GroupId == groupId && x.PostStatus == GroupPostStatus.Approved)
            .OrderByDescending(x => x.UpdatedAt)
            .ThenByDescending(x => x.TotalLikes)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new GroupPost
            {
                Id = x.Id,
                Content = x.Content,
                CreatedBy = x.CreatedBy,
                AllowComment = x.AllowComment,
                TotalLikes = x.TotalLikes,
                CreatedAt = x.CreatedAt,
                Creator = new UserProfile
                {
                    UserId = x.Creator.UserId,
                    NickName = x.Creator.NickName,
                    AvatarUrl = x.Creator.AvatarUrl,
                },
                AttachedMedias = x.AttachedMedias.Select(y => new GroupPostAttachedMedia
                {
                    FileName = y.FileName,
                    MediaType = y.MediaType,
                    StorageUrl = y.StorageUrl,
                    UploadOrder = y.UploadOrder,
                }),
                UserLikes = x.UserLikes.Where(l => l.UserId == userId).ToList(),
                Group = new SocialGroup
                {
                    Id = x.Group.Id,
                    Name = x.Group.Name,
                    IsPublic = x.Group.IsPublic,
                    CoverPhotoUrl = x.Group.CoverPhotoUrl,
                },
            })
            .ToListAsync(token);
    }
}
