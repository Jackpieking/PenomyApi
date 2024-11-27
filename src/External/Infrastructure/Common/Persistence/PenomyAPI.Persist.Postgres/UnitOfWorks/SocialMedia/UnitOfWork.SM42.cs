using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM42Repository _SM42Repository;

    public ISM42Repository SM42Repository
    {
        get
        {
            _SM42Repository ??= new SM42Repository(_dbContext);

            return _SM42Repository;
        }
    }
}
