using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IFeatG3Repository _featG3Repository;

    public IFeatG3Repository FeatG3Repository
    {
        get
        {
            if (Equals(_featG3Repository, null))
            {
                _featG3Repository = new FeatG3Repository(_dbContext);
            }

            return _featG3Repository;
        }
    }
}
