using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG37Repository _G37Repository;

    public IG37Repository G37Repository
    {
        get
        {
            if (Equals(_G37Repository, null))
            {
                _G37Repository = new G37Repository(_dbContext);
            }

            return _G37Repository;
        }
    }
}
