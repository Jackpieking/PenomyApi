using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG2Repository _G2Repository;

    public IG2Repository G2Repository
    {
        get
        {
            if (Equals(_G2Repository, null))
            {
                _G2Repository = new G2Repository(_dbContext);
            }

            return _G2Repository;
        }
    }
}
