using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG48Repository _G48Repository;

    public IG48Repository G48Repository
    {
        get
        {
            _G48Repository ??= new G48Repository(_dbContext);

            return _G48Repository;
        }
    }
}
