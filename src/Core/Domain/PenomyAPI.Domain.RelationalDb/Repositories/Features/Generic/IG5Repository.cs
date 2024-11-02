using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default);
    Task<Artwork> GetArtWorkDetailByIdAsync(long artworkId, CancellationToken ct = default);
    Task<bool> IsArtworkFavoriteAsync(long userId, long artworkId, CancellationToken ct = default);
}
