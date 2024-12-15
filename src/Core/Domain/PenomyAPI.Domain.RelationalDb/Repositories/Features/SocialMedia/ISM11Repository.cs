using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM11Repository
{
    Task<List<GroupPost>> GetGroupPostsAsync(long userId, long groupId, CancellationToken token);
}
