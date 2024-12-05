using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM25Repository
{
    Task<bool> UpdatePostCommentsAsync(
        long commentId,
        string content,
        long userId,
        CancellationToken cancellationToken
    );
}
