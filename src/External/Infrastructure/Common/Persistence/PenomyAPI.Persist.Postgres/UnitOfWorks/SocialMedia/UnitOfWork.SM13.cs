using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM13Repository _featSM13Repository;

    public ISM13Repository FeatSM13Repository
    {
        get
        {
            if (Equals(_featSM13Repository, null)) _featSM13Repository = new SM13Repository(_dbContext);

            return _featSM13Repository;
        }
    }
}
