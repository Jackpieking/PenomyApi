using System.Collections.Generic;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG21Repository
{
    Task<List<ArtworkComment>> GetCommentsAsync(long chapterId);
}
