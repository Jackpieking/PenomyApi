using System.Collections.Generic;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG28Repository
{
    Task<int> GetPaginationOptionsByArtworkTypeAsync(ArtworkType artworkType, long creatorId);

    Task<List<G28ArtworkDetailReadModel>> GetPaginationDetailAsync(
        long creatorId,
        ArtworkType artworkType,
        int pageNumber,
        int pageSize
    );
}
