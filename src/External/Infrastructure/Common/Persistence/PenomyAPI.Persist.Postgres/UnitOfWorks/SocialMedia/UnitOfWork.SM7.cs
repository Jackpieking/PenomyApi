using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM7Repository _SM7Repository;

    public ISM7Repository SM7Repository
    {
        get
        {
            _SM7Repository ??= new SM7Repository(_dbContext);

            return _SM7Repository;
        }
    }
}
