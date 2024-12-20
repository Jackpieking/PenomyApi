using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM24Repository
{
    Task<long> CreateUserPostCommentsAsync(
        UserPostComment comment,
        CancellationToken cancellationToken
    );
    Task<long> CreateGroupPostCommentsAsync(
        GroupPostComment comment,
        CancellationToken cancellationToken
    );
}
