using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt4Repository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken);

    Task<IEnumerable<ArtworkOrigin>> GetAllOriginsAsync(CancellationToken cancellationToken);

    Task<bool> CreateComicAsync(
        Artwork comic,
        ArtworkMetaData comicMetaData,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken
    );
}
