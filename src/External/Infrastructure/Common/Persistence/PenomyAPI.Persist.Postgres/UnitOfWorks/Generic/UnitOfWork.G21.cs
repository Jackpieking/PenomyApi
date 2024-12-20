using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG21Repository _G21Repository;

    public IG21Repository G21Repository
    {
        get
        {
            if (Equals(_G21Repository, null))
            {
                _G21Repository = new G21Repository(_dbContext);
            }

            return _G21Repository;
        }
    }
}
