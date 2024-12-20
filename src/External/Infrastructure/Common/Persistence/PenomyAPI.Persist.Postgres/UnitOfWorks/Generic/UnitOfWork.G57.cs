using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG57Repository _G57Repository;

    public IG57Repository G57Repository
    {
        get
        {
            if (Equals(_G57Repository, null))
            {
                _G57Repository = new G57Repository(_dbContext);
            }

            return _G57Repository;
        }
    }
}
