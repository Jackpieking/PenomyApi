using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public sealed class FeatG1Repository : IFeatG1Repository
{
    private readonly DbContext _dbContext;

    public FeatG1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
