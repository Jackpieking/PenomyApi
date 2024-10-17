using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG13Repository _G13Repository;

    public IG13Repository G13Repository
    {
        get
        {
            if (Equals(_G13Repository, null))
            {
                _G13Repository = new G13Repository(_dbContext);
            }

            return _G13Repository;
        }
    }
}
