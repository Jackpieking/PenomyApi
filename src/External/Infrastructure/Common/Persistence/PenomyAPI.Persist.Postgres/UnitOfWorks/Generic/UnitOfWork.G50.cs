using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG50Repository _G50Repository;

    public IG50Repository G50Repository
    {
        get
        {
            if (Equals(_G50Repository, null)) _G50Repository = new G50Repository(_dbContext);

            return _G50Repository;
        }
    }
}
