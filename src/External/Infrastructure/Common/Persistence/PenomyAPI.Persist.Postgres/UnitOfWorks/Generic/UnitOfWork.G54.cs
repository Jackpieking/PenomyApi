using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG54Repository _G54Repository;

    public IG54Repository G54Repository
    {
        get
        {
            if (Equals(_G54Repository, null))
            {
                _G54Repository = new G54Repository(_dbContext);
            }

            return _G54Repository;
        }
    }
}
