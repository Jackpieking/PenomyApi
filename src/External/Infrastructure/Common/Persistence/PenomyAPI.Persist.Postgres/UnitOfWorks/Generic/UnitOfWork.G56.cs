using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG56Repository _G56Repository;

    public IG56Repository G56Repository
    {
        get
        {
            if (Equals(_G56Repository, null))
            {
                _G56Repository = new G56Repository(_dbContext);
            }

            return _G56Repository;
        }
    }
}
