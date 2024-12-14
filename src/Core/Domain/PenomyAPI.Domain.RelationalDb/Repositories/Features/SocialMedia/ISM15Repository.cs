using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM15Repository
{
    Task<List<(bool, long)>> IsLikePostAsync(long userId, List<string> postIds, CancellationToken token);
    Task<List<UserPost>> GetPersonalPostsAsync(long userId, CancellationToken token);
}
