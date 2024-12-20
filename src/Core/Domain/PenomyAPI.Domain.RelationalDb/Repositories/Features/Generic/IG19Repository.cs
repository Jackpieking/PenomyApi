using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG19Repository
{
    Task<G19AnimeChapterDetailReadModel> GetChapterDetailByIdAsync(
        long chapterId,
        CancellationToken cancellationToken);

    Task<List<G19AnimeChapterItemReadModel>> GetAllChaptersAsyncByAnimeId(
        long animeId,
        CancellationToken cancellationToken);
}
