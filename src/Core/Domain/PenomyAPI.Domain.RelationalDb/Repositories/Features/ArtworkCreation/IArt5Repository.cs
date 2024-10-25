using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt5Repository
{
    Task<Artwork> GetComicDetailByIdAsync(
        long comicId,
        CancellationToken cancellationToken);
}
