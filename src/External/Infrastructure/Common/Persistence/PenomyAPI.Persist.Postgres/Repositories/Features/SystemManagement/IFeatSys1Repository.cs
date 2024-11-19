using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SystemManagement;

public class FeatSys1Repository : IFeatSys1Repository
{
    private readonly DbContext _dbContext;

    public FeatSys1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
