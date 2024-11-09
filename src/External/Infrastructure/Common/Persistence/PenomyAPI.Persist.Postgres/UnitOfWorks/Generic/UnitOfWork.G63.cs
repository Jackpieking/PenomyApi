using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG63Repository _G63Repository;

    public IG63Repository G63Repository
    {
        get
        {
            _G63Repository ??= new G63Repository(_dbContext);

            return _G63Repository;
        }
    }
}
