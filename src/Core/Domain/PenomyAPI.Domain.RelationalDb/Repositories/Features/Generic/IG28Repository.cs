using System.Collections.Generic;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG28Repository
{
    Task<long> GetPaginationOptionsByArtworkTypeAsync(long ArtworkType, long UserId);
    Task<List<Artwork>> GetPaginationDetailAsync(
        long UserId,
        long ArtworkType,
        int PageNumber,
        int PageSize
    );
}
