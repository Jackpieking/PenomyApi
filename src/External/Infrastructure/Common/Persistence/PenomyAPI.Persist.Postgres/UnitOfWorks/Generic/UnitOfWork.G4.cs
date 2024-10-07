using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG4Repository _G4Repository;

    public IG4Repository G4Repository
    {
        get
        {
            if (Equals(_G4Repository, null))
            {
                _G4Repository = new G4Repository(_dbContext);
            }

            return _G4Repository;
        }
    }
}
