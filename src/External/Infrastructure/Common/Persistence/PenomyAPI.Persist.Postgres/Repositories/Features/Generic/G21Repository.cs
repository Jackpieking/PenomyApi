using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G21Repository : IG21Repository
{
    private readonly DbSet<UserLikeArtworkComment> _userLikeArtworkCommentDbSet;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<ArtworkChapter> _artworkChapterDbSet;

    public G21Repository(DbContext dbContext)
    {
        _userLikeArtworkCommentDbSet = dbContext.Set<UserLikeArtworkComment>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _artworkChapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public async Task<List<ArtworkComment>> GetCommentsAsync(long ChapterId, long UserId)
    {
        var artwork = await _artworkChapterDbSet
            .Where(c => c.Id == ChapterId)
            .Select(c => new Artwork { CreatedBy = c.BelongedArtwork.CreatedBy })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        var result = await _artworkCommentDbSet
            .Where(acr => acr.ChapterId == ChapterId)
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
                    .FirstOrDefault(),
            })
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
