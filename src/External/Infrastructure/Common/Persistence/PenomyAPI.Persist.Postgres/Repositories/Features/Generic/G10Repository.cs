using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G10Repository : IG10Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<ArtworkCommentReference> _artworkCommentReferenceDbSet;
    private readonly DbSet<ArtworkCommentParentChild> _artworkCommentParentChildDbSet;

    public G10Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _artworkCommentReferenceDbSet = dbContext.Set<ArtworkCommentReference>();
        _artworkCommentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
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
                    NickName = c.Creator.NickName
                },


            }).AsNoTracking().ToListAsync();
        return result;
    }
}
