using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG36Repository
{
    Task<bool> IsNickNameAlreadyExistedAsync(
        long userId,
        string nickName,
        CancellationToken cancellationToken);

    Task<bool> UpdateProfileAsync(
        UserProfile userProfile,
        CancellationToken cancellationToken);
}
