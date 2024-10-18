using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG43Repository _G43Repository;

    public IG43Repository G43Repository
    {
        get
        {
            if (Equals(_G43Repository, null))
            {
                _G43Repository = new G43Repository(_dbContext);
            }

            return _G43Repository;
        }
    }
}
