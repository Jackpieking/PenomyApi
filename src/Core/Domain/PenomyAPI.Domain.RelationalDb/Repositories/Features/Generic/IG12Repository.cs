using System.Collections.Generic;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic
{
    public interface IG12Repository
    {
        Task<List<ArtworkCategory>> GetAnimesByCategoryAsync(long CategoryId);
    }
}
