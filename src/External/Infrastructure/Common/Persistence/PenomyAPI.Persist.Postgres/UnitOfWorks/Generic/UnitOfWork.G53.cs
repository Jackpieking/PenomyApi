using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG53Repository _G53Repository;

    public IG53Repository G53Repository
    {
        get
        {
            if (Equals(_G53Repository, null))
            {
                _G53Repository = new G53Repository(_dbContext);
            }

            return _G53Repository;
        }
    }
}
