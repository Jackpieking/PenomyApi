using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG52Repository _G52Repository;

    public IG52Repository G52Repository
    {
        get
        {
            if (Equals(_G52Repository, null))
            {
                _G52Repository = new G52Repository(_dbContext);
            }

            return _G52Repository;
        }
    }
}
