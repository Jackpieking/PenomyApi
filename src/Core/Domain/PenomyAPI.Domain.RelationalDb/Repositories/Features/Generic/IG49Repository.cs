using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG49Repository
{
    Task<bool> IsArtworkExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> RateArtworkAsync(long userId, long artworkId, byte starRates, CancellationToken cancellationToken);
}
