using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IFeatG1Repository _featG1Repository;

    public IFeatG1Repository FeatG1Repository
    {
        get
        {
            if (Equals(_featG1Repository, null))
            {
                _featG1Repository = new FeatG1Repository(_dbContext);
            }

            return _featG1Repository;
        }
    }
}
