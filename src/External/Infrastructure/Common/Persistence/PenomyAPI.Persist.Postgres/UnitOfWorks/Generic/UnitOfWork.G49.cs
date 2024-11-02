using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG49Repository _G49Repository;

    public IG49Repository G49Repository
    {
        get
        {
            if (Equals(_G49Repository, null))
            {
                _G49Repository = new G49Repository(_dbContext);
            }

            return _G49Repository;
        }
    }
}
