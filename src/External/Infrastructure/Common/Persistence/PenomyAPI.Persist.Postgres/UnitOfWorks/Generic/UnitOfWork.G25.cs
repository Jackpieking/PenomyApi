using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IG25Repository _g25Repository;
    public IG25Repository G25Repository
    {
        get
        {
            if (_g25Repository == null)
            {
                _g25Repository = new G25Repository(_dbContext);
            }
            return _g25Repository;
        }
    }
}
