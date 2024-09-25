using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class FeatG3Repository : IFeatG3Repository
{
    private readonly DbContext _dbContext;

    public FeatG3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<object> GetRecommendedRecentlyUpdatedComicsAsync()
    {
        return Task.FromResult(new object());
    }

}
