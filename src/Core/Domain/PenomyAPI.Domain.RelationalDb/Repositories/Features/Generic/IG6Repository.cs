using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG6Repository
{
    Task<List<Artwork>> GetRecommendedArtworksAsync(
        long artworkId,
        int totalRecommendedArtworks,
        CancellationToken cancellationToken
    );
}
