using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG44Repository _G44Repository;

    public IG44Repository G44Repository
    {
        get
        {
            if (Equals(_G44Repository, null))
            {
                _G44Repository = new G44Repository(_dbContext);
            }

            return _G44Repository;
        }
    }
}
