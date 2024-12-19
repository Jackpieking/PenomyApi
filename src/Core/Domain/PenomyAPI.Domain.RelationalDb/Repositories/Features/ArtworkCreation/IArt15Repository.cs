using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt15Repository
{
    Task<bool> CreateAnimeAsync(
        Artwork anime,
        ArtworkMetaData metaData,
        IEnumerable<ArtworkCategory> artworkCategories,
        CancellationToken cancellationToken
    );
}
