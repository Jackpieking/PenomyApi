using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG57Repository
{
    Task<long> ExcecuteUnlikeCommentAsync(
        long CommentId,
        long UserId,
        CancellationToken cancellationToken
    );
}
