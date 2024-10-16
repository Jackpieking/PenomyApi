using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG20Repository _G20Repository;

    public IG20Repository G20Repository
    {
        get
        {
            if (Equals(_G20Repository, null))
            {
                _G20Repository = new G20Repository(_dbContext);
            }

            return _G20Repository;
        }
    }
}
