using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG59Repository _G59Repository;

    public IG59Repository G59Repository
    {
        get
        {
            if (Equals(_G59Repository, null))
            {
                _G59Repository = new G59Repository(_dbContext);
            }

            return _G59Repository;
        }
    }
}
