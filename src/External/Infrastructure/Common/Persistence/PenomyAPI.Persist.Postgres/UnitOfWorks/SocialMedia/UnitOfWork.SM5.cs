using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM5Repository _SM5Repository;

    public ISM5Repository SM5Repository
    {
        get
        {
            _SM5Repository ??= new SM5Repository(_dbContext);

            return _SM5Repository;
        }
    }
}
