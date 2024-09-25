using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public sealed class FeatSM1Repository : IFeatSM1Repository
{
    private readonly DbContext _dbContext;

    public FeatSM1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
