using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM24Repository
{
    Task<long> CreatePostCommentsAsync(
        UserPostComment comment,
        CancellationToken cancellationToken
    );
}
