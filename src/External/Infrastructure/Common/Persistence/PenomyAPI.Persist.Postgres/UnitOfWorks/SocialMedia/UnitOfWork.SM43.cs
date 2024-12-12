using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM43Repository _SM43Repository;

    public ISM43Repository SM43Repository
    {
        get
        {
            _SM43Repository ??= new SM43Repository(_dbContext);

            return _SM43Repository;
        }
    }
}
