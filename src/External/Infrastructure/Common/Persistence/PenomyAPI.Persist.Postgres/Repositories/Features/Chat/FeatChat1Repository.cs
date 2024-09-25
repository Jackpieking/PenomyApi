using Microsoft.EntityFrameworkCore;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public sealed class FeatChat1Repository : IFeatChat1Repository
{
    private readonly DbContext _dbContext;

    public FeatChat1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
