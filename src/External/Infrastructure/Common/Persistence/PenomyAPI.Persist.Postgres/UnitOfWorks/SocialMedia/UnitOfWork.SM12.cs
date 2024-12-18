using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM12Repository _featSM12Repository;

    public ISM12Repository FeatSM12Repository
    {
        get
        {
            if (Equals(_featSM12Repository, null))
                _featSM12Repository = new SM12Repository(_dbContext);

            return _featSM12Repository;
        }
    }
}
