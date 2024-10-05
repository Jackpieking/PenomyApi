using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG7Repository _featG7Repository;
    public IG7Repository G7Repository
    {
        get
        {
            if (Equals(_featG7Repository, null))
            {
                _featG7Repository = new G7Repository(_dbContext);
            }

            return _featG7Repository;
        }
    }
}
