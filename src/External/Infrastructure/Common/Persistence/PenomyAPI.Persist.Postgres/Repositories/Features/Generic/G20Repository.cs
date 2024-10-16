using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G20Repository : IG20Repository
{
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;

    public G20Repository(DbContext dbContext)
    {
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
    }

    public async Task<List<ArtworkComment>> GetCommentsAsync(long ArtworkId)
    {
        var result = await _artworkCommentDbSet
            .Where(acr => acr.ArtworkId == ArtworkId)
            .Select(c => new ArtworkComment
            {
                Id = c.Id,
                Content = c.Content,
                IsDirectlyCommented = c.IsDirectlyCommented,
                TotalChildComments = c.TotalChildComments,
                TotalLikes = c.TotalLikes,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Creator = new UserProfile
                {
                    NickName = c.Creator.NickName,
                    AvatarUrl = c.Creator.AvatarUrl,
                },
            })
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
