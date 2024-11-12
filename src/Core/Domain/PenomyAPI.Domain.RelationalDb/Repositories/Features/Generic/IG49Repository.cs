using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG49Repository
{
    Task<bool> IsArtworkExistsAsync(long id, CancellationToken cancellationToken);

    Task<ArtworkMetaData> RateArtworkAsync(long userId, long artworkId, byte starRates,
        CancellationToken cancellationToken);

    Task<byte> GetCurrentUserRatingAsync(long userId, long artworkId, CancellationToken cancellationToken);
}
