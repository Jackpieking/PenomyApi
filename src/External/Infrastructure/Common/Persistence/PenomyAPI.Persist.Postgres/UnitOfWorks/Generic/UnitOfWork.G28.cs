using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG28Repository _G28Repository;

    public IG28Repository G28Repository
    {
        get
        {
            if (Equals(_G28Repository, null))
            {
                _G28Repository = new G28Repository(_dbContext);
            }

            return _G28Repository;
        }
    }
}
