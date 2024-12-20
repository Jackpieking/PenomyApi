using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG35Repository _G35Repository;

    public IG35Repository G35Repository
    {
        get
        {
            if (Equals(_G35Repository, null))
            {
                _G35Repository = new G35Repository(_dbContext);
            }

            return _G35Repository;
        }
    }
}
