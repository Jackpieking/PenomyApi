using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt8Repository
{
    Task<bool> TemporarilyRemoveArtworkByIdAsync(
        long artworkId,
        long removedBy,
        DateTime removedAt,
        CancellationToken cancellationToken);
}
