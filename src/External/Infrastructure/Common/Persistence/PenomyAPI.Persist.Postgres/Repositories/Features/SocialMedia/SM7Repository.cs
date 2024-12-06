using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia
{
    public class SM7Repository : ISM7Repository
    {
        private readonly DbSet<SocialGroup> _socialGroups;

        public SM7Repository(DbContext dbContext)
        {
            _socialGroups = dbContext.Set<SocialGroup>();
        }

        public async Task<int> GetTotalOfArtworksByTypeAndUserIdAsync(long userId, CancellationToken ct)
        {
            return await _socialGroups
                .AsNoTracking()
                .CountAsync(o =>
                    o.GroupMembers.Any(gm => gm.MemberId == userId) &&
                    o.GroupStatus == SocialGroupStatus.Active,
                    cancellationToken: ct);
        }

        public async Task<ICollection<SocialGroup>> GetJoinedGroupsByUserIdWithPaginationAsync(
            long userId,
            int pageNum,
            int groupNum,
            CancellationToken ct
        )
        {
            return await _socialGroups
                .AsNoTracking()
                .Where(o =>
                    o.GroupMembers.Any(gm => gm.MemberId == userId) &&
                    o.GroupStatus == SocialGroupStatus.Active)
                .OrderByDescending(o => o.GroupMembers.FirstOrDefault(gm => gm.MemberId == userId).JoinedAt)
                .Skip((pageNum - 1) * groupNum)
                .Take(groupNum)
                .Select(o => new SocialGroup
                {
                    Id = o.Id,
                    Name = o.Name,
                    IsPublic = o.IsPublic,
                    Description = o.Description,
                    CoverPhotoUrl = o.CoverPhotoUrl,
                    TotalMembers = o.TotalMembers,
                    RequireApprovedWhenPost = o.RequireApprovedWhenPost,
                    GroupStatus = o.GroupStatus,
                    CreatedBy = o.CreatedBy,
                    CreatedAt = o.CreatedAt,
                    // Act as group activity time
                    Creator = new Domain.RelationalDb.Entities.Generic.UserProfile
                    {
                        UpdatedAt = o.GroupMembers.FirstOrDefault(gm => gm.MemberId == userId).JoinedAt
                    }
                })
                .ToListAsync(cancellationToken: ct);
        }
    }
}
