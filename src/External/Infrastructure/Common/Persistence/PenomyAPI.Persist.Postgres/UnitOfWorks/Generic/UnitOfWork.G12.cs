using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG12Repository _G12Repository;

    public IG12Repository G12Repository
    {
        get
        {
            if (Equals(_G12Repository, null))
            {
                _G12Repository = new G12Repository(_dbContext);
            }

            return _G12Repository;
        }
    }
}
