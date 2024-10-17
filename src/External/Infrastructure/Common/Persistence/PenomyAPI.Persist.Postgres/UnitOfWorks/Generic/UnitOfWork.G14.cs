using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG14Repository _G14Repository;

    public IG14Repository G14Repository
    {
        get
        {
            if (Equals(_G14Repository, null))
            {
                _G14Repository = new G14Repository(_dbContext);
            }

            return _G14Repository;
        }
    }
}
