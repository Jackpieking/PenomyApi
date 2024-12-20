using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM14Repository
{
    Task<List<long>> GetAttachedFileIdAsync(long userId, CancellationToken cancellationToken);
    Task<List<long>> GetGroupPostAttachedFileIdAsync(
        long userId,
        CancellationToken cancellationToken
    );
    Task<bool> IsExistUserPostAsync(long id, long userId, CancellationToken cancellationToken);
    Task<bool> RemoveUserPostAsync(
        long id,
        long userId,
        bool isGroupPost,
        CancellationToken cancellationToken
    );
}
