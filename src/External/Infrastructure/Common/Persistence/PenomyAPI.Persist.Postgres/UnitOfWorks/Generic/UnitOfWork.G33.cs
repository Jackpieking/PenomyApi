using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG33Repository _g33Repository;

    public IG33Repository G33Repository
    {
        get
        {
            _g33Repository ??= new G33Repository(_dbContext);

            return _g33Repository;
        }
    }
}
