using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG34Repository _g34Repository;

    public IG34Repository G34Repository
    {
        get
        {
            _g34Repository ??= new G34Repository(_dbContext, _userManager);

            return _g34Repository;
        }
    }
}
