using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM23Repository
{
    Task<List<UserPostComment>> GetUserPostCommentsAsync(
        long PostId,
        long UserId,
        CancellationToken cancellationToken
    );
}
