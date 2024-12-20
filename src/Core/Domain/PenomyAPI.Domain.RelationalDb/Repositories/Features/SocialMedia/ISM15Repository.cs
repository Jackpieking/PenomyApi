using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM15Repository
{
    Task<List<UserPost>> GetPersonalPostsAsync(long userId, CancellationToken token);
    Task<List<UserPost>> GetUserPostsAsync(long userId, CancellationToken token);
}
