using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG58Repository _G58Repository;

    public IG58Repository G58Repository
    {
        get
        {
            if (Equals(_G58Repository, null))
            {
                _G58Repository = new G58Repository(_dbContext);
            }

            return _G58Repository;
        }
    }
}
