using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt4Repository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken);

    Task<bool> CreateComicAsync(
        Artwork comic,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken);
}