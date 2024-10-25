using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G10Repository : IG10Repository
{
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<UserLikeArtworkComment> _userLikeArtworkCommentDbSet;

    public G10Repository(DbContext dbContext)
    {
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _userLikeArtworkCommentDbSet = dbContext.Set<UserLikeArtworkComment>();
    }

    public async Task<List<ArtworkComment>> GetCommentsAsync(long ArtworkId, long UserId)
    {
        var result = await _artworkCommentDbSet
            .Where(acr => acr.ArtworkId == ArtworkId)
            .GroupJoin(
                _userLikeArtworkCommentDbSet,
                c => c.Id,
                u => u.CommentId,
                (c, u) => new { c, u }
            )
            // c: ArtworkComment, u: UserLikeArtworkComment
            .Select(x => new ArtworkComment
            {
                Id = x.c.Id,
                Content = x.c.Content,
                IsDirectlyCommented = x.c.IsDirectlyCommented,
                TotalChildComments = x.c.TotalChildComments,
                TotalLikes = x.c.TotalLikes,
                CreatedAt = x.c.CreatedAt,
                UpdatedAt = x.c.UpdatedAt,
                Creator = new UserProfile
                {
                    UserId = x.c.Creator.UserId,
                    NickName = x.c.Creator.NickName,
                    AvatarUrl = x.c.Creator.AvatarUrl,
                },
                UserLikeArtworkComment = x
                    .u.Select(u => new UserLikeArtworkComment { UserId = u.UserId })
                    .Where(u => u.UserId == UserId)
                    .ToList(),
            })
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
