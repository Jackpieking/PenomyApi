using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG8Repository _featG8Repository;
    public IG8Repository G8Repository
    {
        get
        {
            if (Equals(_featG8Repository, null))
            {
                _featG8Repository = new G8Repository(_dbContext);
            }

            return _featG8Repository;
        }
    }
}
