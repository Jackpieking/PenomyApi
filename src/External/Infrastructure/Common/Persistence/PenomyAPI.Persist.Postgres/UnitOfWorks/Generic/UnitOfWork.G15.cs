using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG15Repository _G15Repository;

    public IG15Repository G15Repository
    {
        get
        {
            if (Equals(_G15Repository, null))
            {
                _G15Repository = new G15Repository(_dbContext);
            }

            return _G15Repository;
        }
    }
}
