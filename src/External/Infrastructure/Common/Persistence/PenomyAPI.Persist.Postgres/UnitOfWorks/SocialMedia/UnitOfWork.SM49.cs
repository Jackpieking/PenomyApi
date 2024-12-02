using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM49Repository _SM49Repository;

    public ISM49Repository SM49Repository
    {
        get
        {
            _SM49Repository ??= new SM49Repository(_dbContext);

            return _SM49Repository;
        }
    }
}
