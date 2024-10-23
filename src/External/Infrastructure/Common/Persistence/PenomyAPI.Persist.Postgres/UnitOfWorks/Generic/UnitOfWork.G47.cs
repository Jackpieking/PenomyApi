using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG47Repository _G47Repository;

    public IG47Repository G47Repository
    {
        get
        {
            if (Equals(_G47Repository, null))
            {
                _G47Repository = new G47Repository(_dbContext);
            }

            return _G47Repository;
        }
    }
}
