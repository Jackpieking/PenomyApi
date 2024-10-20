using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG19Repository _G19Repository;

    public IG19Repository G19Repository
    {
        get
        {
            if (Equals(_G19Repository, null))
            {
                _G19Repository = new G19Repository(_dbContext);
            }

            return _G19Repository;
        }
    }
}
