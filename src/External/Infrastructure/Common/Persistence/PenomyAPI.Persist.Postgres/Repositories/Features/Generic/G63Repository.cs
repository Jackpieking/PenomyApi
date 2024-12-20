using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G63Repository : IG63Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserFollowedCreator> _userFollowedCreators;
    private readonly DbSet<ChatGroup> _chatGroup;

    public G63Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userFollowedCreators = dbContext.Set<UserFollowedCreator>();
        _chatGroup = dbContext.Set<ChatGroup>();
    }

    public async Task<ICollection<CreatorProfile>> GetFollowedCreatorsByUserIdWithPaginationAsync(
        long userId,
        int pageNum,
        int creatorNum,
        CancellationToken ct
    )
    {
        return await _userFollowedCreators.AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.StartedAt)
            .Skip((pageNum - 1) * creatorNum)
            .Take(creatorNum)
            .Select(o => new CreatorProfile
            {
                CreatorId = o.CreatorId,
                TotalArtworks = o.Creator.CreatorProfile.TotalArtworks,
                TotalFollowers = o.Creator.CreatorProfile.TotalFollowers,
                ProfileOwner = new UserProfile
                {
                    NickName = o.Creator.NickName,
                    AvatarUrl = o.Creator.AvatarUrl,
                }
            })
            .ToListAsync(ct);
    }

    public async Task<int> GetTotalOfCreatorByUserIdAsync(
        long userId,
        CancellationToken ct
    )
    {
        return await _userFollowedCreators.CountAsync(o => o.UserId == userId, ct);
    }
}
