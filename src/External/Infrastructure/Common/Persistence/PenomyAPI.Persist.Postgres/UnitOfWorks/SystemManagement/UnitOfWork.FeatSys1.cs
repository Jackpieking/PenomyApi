using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;
using PenomyAPI.Persist.Postgres.Repositories.Features.SystemManagement;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IFeatSys1Repository _featSys1Repository;

    public IFeatSys1Repository FeatSys1Repository
    {
        get
        {
            if (Equals(_featSys1Repository, null))
            {
                _featSys1Repository = new FeatSys1Repository(_dbContext);
            }

            return _featSys1Repository;
        }
    }
}
