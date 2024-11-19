using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM1Repository _SM1Repository;

    public ISM1Repository SM1Repository
    {
        get
        {
            _SM1Repository ??= new SM1Repository(_dbContext);

            return _SM1Repository;
        }
    }
}
