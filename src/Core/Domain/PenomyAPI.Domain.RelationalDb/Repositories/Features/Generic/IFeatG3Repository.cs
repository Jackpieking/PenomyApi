using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IFeatG3Repository
{
    Task<object> GetRecommendedRecentlyUpdatedComicsAsync();
}
