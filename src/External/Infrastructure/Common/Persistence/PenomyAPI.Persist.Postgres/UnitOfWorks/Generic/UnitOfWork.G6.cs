using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG6Repository _featG6Repository;
    public IG6Repository G6Repository
    {
        get
        {
            if (Equals(_featG6Repository, null))
            {
                _featG6Repository = new G6Repository(_dbContext);
            }

            return _featG6Repository;
        }
    }
}
