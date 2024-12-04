using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM23Repository _SM23Repository;

    public ISM23Repository SM23Repository
    {
        get
        {
            _SM23Repository ??= new SM23Repository(_dbContext);

            return _SM23Repository;
        }
    }
}
