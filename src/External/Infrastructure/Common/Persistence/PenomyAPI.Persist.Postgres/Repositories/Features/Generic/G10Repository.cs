using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
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

    public async Task<List<ArtworkComment>> GetCommentsAsync(
        long ArtworkId,
        long UserId,
        int CommentSection
    )
    {
        var result = new Result<List<ArtworkComment>>();
        if (CommentSection == 1)
        {
            await GetArtworkCommentsDefaultAsync(ArtworkId, UserId, result);
        }
        else if (CommentSection == 2)
        {
            await GetTopCommentsAsync(ArtworkId, UserId, result);
        }
        else if (CommentSection == 3)
        {
            await GetNewCommentsAsync(ArtworkId, UserId, result);
        }
        else
            result = null;
        return result.Value;
    }

    public async Task GetArtworkCommentsDefaultAsync(
        long ArtworkId,
        long UserId,
        Result<List<ArtworkComment>> comments
    )
    {
        comments.Value = await _artworkCommentDbSet
            .Where(acr =>
                acr.ArtworkId == ArtworkId
                && acr.IsDirectlyCommented == true
                && acr.IsRemoved == false
            )
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
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task GetTopCommentsAsync(
        long ArtworkId,
        long UserId,
        Result<List<ArtworkComment>> comments
    )
    {
        comments.Value = await _artworkCommentDbSet
            .Where(acr =>
                acr.ArtworkId == ArtworkId
                && acr.IsDirectlyCommented == true
                && acr.IsRemoved == false
            )
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
            .OrderByDescending(x => x.TotalLikes)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task GetNewCommentsAsync(
        long ArtworkId,
        long UserId,
        Result<List<ArtworkComment>> comments
    )
    {
        comments.Value = await _artworkCommentDbSet
            .Where(acr =>
                acr.ArtworkId == ArtworkId
                && acr.IsDirectlyCommented == true
                && acr.IsRemoved == false
            )
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
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
