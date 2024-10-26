using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG58Repository
{
    Task<long> ExcecuteReplyCommentAsync(
        ArtworkComment comment,
        long parentCommentId,
        CancellationToken cancellationToken
    );
}
