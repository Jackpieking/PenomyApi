using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG45Repository _G45Repository;

    public IG45Repository G45Repository
    {
        get
        {
            if (Equals(_G45Repository, null))
            {
                _G45Repository = new G45Repository(_dbContext);
            }

            return _G45Repository;
        }
    }
}
