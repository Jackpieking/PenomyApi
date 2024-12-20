using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM27Repository _SM27Repository;

    public ISM27Repository SM27Repository
    {
        get
        {
            _SM27Repository ??= new SM27Repository(_dbContext);

            return _SM27Repository;
        }
    }
}
