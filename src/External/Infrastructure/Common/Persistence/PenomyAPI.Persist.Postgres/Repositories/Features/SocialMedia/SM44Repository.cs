using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM44Repository : ISM44Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;
    private readonly DbContext _dbContext;

    public SM44Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
    }

    public async Task<bool> CreateGroupJoinRequestAsync(long groupId, long userId)
    {
        try
        {
            await _socialGroupJoinRequestDbSet.AddAsync(
                new SocialGroupJoinRequest
                {
                    GroupId = groupId,
                    CreatedBy = userId,
                    RequestStatus = RequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }
            );
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
