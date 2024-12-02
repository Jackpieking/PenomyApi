using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM45Repository _SM45Repository;

    public ISM45Repository SM45Repository
    {
        get
        {
            _SM45Repository ??= new SM45Repository(_dbContext);

            return _SM45Repository;
        }
    }
}
