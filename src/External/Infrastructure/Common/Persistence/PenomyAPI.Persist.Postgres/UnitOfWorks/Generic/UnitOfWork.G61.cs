using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG61Repository _G61Repository;

    public IG61Repository G61Repository
    {
        get
        {
            _G61Repository ??= new G61Repository(_dbContext);

            return _G61Repository;
        }
    }
}
