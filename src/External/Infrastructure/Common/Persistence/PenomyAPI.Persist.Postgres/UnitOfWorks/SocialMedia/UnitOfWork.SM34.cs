using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM34Repository _featSM34Repository;

    public ISM34Repository FeatSM34Repository
    {
        get
        {
            if (Equals(_featSM34Repository, null))
                _featSM34Repository = new SM34Repository(_dbContext);

            return _featSM34Repository;
        }
    }
}
