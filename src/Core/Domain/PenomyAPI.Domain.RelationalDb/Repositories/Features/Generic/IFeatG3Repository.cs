using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IFeatG3Repository
{
    Task<List<Artwork>> GetRecentlyUpdatedComicsAsync();
}
