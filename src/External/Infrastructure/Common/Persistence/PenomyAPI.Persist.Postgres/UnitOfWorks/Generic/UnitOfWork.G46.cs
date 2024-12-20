using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG46Repository _G46Repository;

    public IG46Repository G46Repository
    {
        get
        {
            if (Equals(_G46Repository, null))
            {
                _G46Repository = new G46Repository(_dbContext, _userManager);
            }

            return _G46Repository;
        }
    }
}
