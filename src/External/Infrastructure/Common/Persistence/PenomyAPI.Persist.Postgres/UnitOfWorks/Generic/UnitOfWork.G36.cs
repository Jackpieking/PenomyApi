using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG36Repository _G36Repository;

    public IG36Repository G36Repository
    {
        get
        {
            if (Equals(_G36Repository, null))
            {
                _G36Repository = new G36Repository(_dbContext);
            }

            return _G36Repository;
        }
    }
}
