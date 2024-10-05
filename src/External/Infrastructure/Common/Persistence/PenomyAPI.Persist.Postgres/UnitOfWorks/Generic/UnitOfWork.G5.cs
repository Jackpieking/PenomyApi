using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG5Repository _featG5Repository;

    public IG5Repository FeatG5Repository
    {
        get
        {
            if (Equals(_featG5Repository, null))
            {
                _featG5Repository = new G5Repository(_dbContext);
            }

            return _featG5Repository;
        }
    }
}
