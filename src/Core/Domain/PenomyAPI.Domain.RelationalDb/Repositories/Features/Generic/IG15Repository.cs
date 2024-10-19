using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG15Repository
{
    Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default);
    Task<Artwork> GetArtWorkDetailByIdAsync(long artworkId, CancellationToken ct = default);
}
