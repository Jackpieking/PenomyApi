using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic
{
    public interface IG7Repository
    {
        Task<List<Artwork>> GetArkworkBySeriesAsync(
            long currentArtworkId,
            int startPage = 1,
            int pageSize = 3,
            CancellationToken cancellationToken = default
        );
        Task<ArtworkMetaData> GetArtworkMetaDataAsync(
            long artworkId,
            CancellationToken token = default
        );
        Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken token = default);
    }
}
