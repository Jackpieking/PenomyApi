using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG10Repository _G10Repository;

    public IG10Repository G10Repository
    {
        get
        {
            if (Equals(_G10Repository, null))
            {
                _G10Repository = new G10Repository(_dbContext);
            }

            return _G10Repository;
        }
    }
}
