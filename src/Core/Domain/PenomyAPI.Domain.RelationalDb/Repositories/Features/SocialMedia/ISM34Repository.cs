using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM34Repository
{
    Task<bool> CreateGroupPostAsync(
        GroupPost createdPost,
        IEnumerable<GroupPostAttachedMedia> postAttachedMediae,
        GroupPostLikeStatistic postLikeStatistic,
        CancellationToken token = default
    );

    Task<UserProfile> GetUserProfileAsync(long userId, CancellationToken token = default);
}
