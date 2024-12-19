using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG15;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG15Repository
{
    Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default);
    
    Task<G15AnimeDetailReadModel> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken ct = default);
}
