using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG56Repository
{
    Task<long> ExcecuteLikeCommentAsync(
        long CommentId,
        long UserId,
        CancellationToken cancellationToken
    );
}
