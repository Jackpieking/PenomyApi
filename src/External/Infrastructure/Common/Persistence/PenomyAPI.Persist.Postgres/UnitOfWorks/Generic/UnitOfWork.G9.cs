using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG9Repository _G9Repository;

    public IG9Repository G9Repository
    {
        get
        {
            if (Equals(_G9Repository, null))
            {
                _G9Repository = new G9Repository(_dbContext);
            }

            return _G9Repository;
        }
    }
}
