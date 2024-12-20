using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt17Repository
{
    Task<Artwork> GetAnimeDetailByIdAsync(long artworkId, CancellationToken cancellationToken);

    Task<bool> CheckCreatorPermissionAsync(
        long artworkId,
        long creatoId,
        CancellationToken cancellationToken);

    Task<bool> UpdateAnimeAsync(
        Artwork animeDetail,
        IEnumerable<ArtworkCategory> artworkCategories,
        bool isThumbnailUpdated,
        bool isCategoriesUpdated,
        CancellationToken cancellationToken
    );
}
