﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM23Repository : ISM23Repository
{
    private readonly DbSet<UserPostComment> _userPostCommentDbSet;
    private readonly DbSet<GroupPostComment> _groupPostCommentDbSet;

    public SM23Repository(DbContext dbContext)
    {
        _userPostCommentDbSet = dbContext.Set<UserPostComment>();
        _groupPostCommentDbSet = dbContext.Set<GroupPostComment>();
    }

    public async Task<List<UserPostComment>> GetUserPostCommentsAsync(
        long PostId,
        long UserId,
        CancellationToken ct
    )
    {
        try
        {
            return await _userPostCommentDbSet
                .Where(o => o.PostId == PostId && o.IsRemoved == false)
                .Select(o => new UserPostComment
                {
                    Id = o.Id,
                    Content = o.Content,
                    CreatedAt = o.CreatedAt,
                    Creator = o.Creator,
                    CreatedBy = o.CreatedBy,
                    IsDirectlyCommented = o.IsDirectlyCommented,
                    TotalChildComments = o.TotalChildComments,
                    TotalLikes = o.TotalLikes,
                    UserLikes = o
                        .UserLikes.Where(l => l.UserId == UserId && l.CommentId == o.Id)
                        .ToList(),
                })
                .AsNoTracking()
                .AsQueryable()
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(ct);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<GroupPostComment>> GetGroupPostCommentsAsync(
        long PostId,
        long UserId,
        CancellationToken ct
    )
    {
        try
        {
            return await _groupPostCommentDbSet
                .Where(o => o.PostId == PostId && o.IsRemoved == false)
                .Select(o => new GroupPostComment
                {
                    Id = o.Id,
                    Content = o.Content,
                    CreatedAt = o.CreatedAt,
                    Creator = o.Creator,
                    CreatedBy = o.CreatedBy,
                    IsDirectlyCommented = o.IsDirectlyCommented,
                    TotalChildComments = o.TotalChildComments,
                    UserLikes = o
                        .UserLikes.Where(l => l.UserId == UserId && l.CommentId == o.Id)
                        .ToList(),
                })
                .AsNoTracking()
                .AsQueryable()
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(ct);
        }
        catch
        {
            return null;
        }
    }
}
