using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM27Repository
{
    Task<bool> TakeDownPostCommentsAsync(
        long commentId,
        CancellationToken cancellationToken
    );

    Task<bool> CheckPostOnwerAsync(
        long postId,
        long userId,
        CancellationToken cancellationToken
    );
}
