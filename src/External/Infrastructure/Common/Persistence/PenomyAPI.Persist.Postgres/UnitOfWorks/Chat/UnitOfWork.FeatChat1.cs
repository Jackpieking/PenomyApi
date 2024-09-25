using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IFeatChat1Repository _featChat1Repository;

    public IFeatChat1Repository FeatChat1Repository
    {
        get
        {
            if (Equals(_featChat1Repository, null))
            {
                _featChat1Repository = new FeatChat1Repository(_dbContext);
            }

            return _featChat1Repository;
        }
    }
}
