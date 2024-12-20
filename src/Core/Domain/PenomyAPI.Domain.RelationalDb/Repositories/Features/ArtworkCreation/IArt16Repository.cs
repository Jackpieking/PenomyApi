using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt16Repository
{
    Task<Artwork> GetAnimeDetailByIdAsync(
        long artworkId,
        CancellationToken cancellationToken);

    Task<List<ArtworkChapter>> GetChaptersByPublishStatusAsync(
        long animeId,
        PublishStatus publishStatus,
        CancellationToken cancellationToken);
}
