using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG62Repository _G62Repository;

    public IG62Repository G62Repository
    {
        get
        {
            _G62Repository ??= new G62Repository(_dbContext);

            return _G62Repository;
        }
    }
}
