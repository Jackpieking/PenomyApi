using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG32Repository _g32Repository;

    public IG32Repository G32Repository
    {
        get
        {
            _g32Repository ??= new G32Repository(_dbContext, _userManager);

            return _g32Repository;
        }
    }
}
