using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G59Repository : IG59Repository
{
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<UserLikeArtworkComment> _userLikeArtworkCommentDbSet;
    private readonly DbSet<ArtworkCommentParentChild> _commentParentChildDbSet;

    public G59Repository(DbContext dbContext)
    {
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _userLikeArtworkCommentDbSet = dbContext.Set<UserLikeArtworkComment>();
        _commentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
    }

    public async Task<List<ArtworkComment>> GetReplyCommentsAsync(long ParentCommentId, long UserId)
    {
        var result = await _commentParentChildDbSet
            .Where(acr => acr.ParentCommentId == ParentCommentId)
            .Select(x => new ArtworkComment
            {
                Id = x.ChildComment.Id,
                Content = x.ChildComment.Content,
                IsDirectlyCommented = x.ChildComment.IsDirectlyCommented,
                TotalChildComments = x.ChildComment.TotalChildComments,
                TotalLikes = x.ChildComment.TotalLikes,
                CreatedAt = x.ChildComment.CreatedAt,
                UpdatedAt = x.ChildComment.UpdatedAt,
                Creator = new UserProfile
                {
                    UserId = x.ChildComment.Creator.UserId,
                    NickName = x.ChildComment.Creator.NickName,
                    AvatarUrl = x.ChildComment.Creator.AvatarUrl,
                },
                UserLikeArtworkComment = x
                    .ChildComment.UserLikeArtworkComment.Select(u => new UserLikeArtworkComment
                    {
                        UserId = u.UserId,
                    })
                    .Where(u => u.UserId == UserId)
                    .ToList(),
            })
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
