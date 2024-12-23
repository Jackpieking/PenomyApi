﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM9Repository : ISM9Repository
{
    private readonly DbSet<SocialGroup> _socialGroupDbSet;

    public SM9Repository(DbContext dbContext)
    {
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
    }

    public async Task<List<SocialGroup>> GetSocialGroupsAsync(
        long userId,
        int maxRecord,
        CancellationToken ct
    )
    {
        try
        {
            var result = await _socialGroupDbSet
                .Where(g => g.CreatedBy == userId)
                .OrderByDescending(g => g.CreatedAt)
                .AsQueryable()
                .AsNoTracking()
                .Take(maxRecord)
                .Select(g => new SocialGroup
                {
                    Id = g.Id,
                    Name = g.Name,
                    CreatedAt = g.CreatedAt,
                    CoverPhotoUrl = g.CoverPhotoUrl,
                    TotalMembers = g.TotalMembers,
                    GroupPosts = g.GroupPosts.ToList(),
                })
                .ToListAsync();

            return result;
        }
        catch
        {
            return null;
        }
    }
}
