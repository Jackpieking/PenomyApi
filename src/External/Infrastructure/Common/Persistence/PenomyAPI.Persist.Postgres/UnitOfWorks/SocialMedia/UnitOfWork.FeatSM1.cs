using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IFeatSM1Repository _featSM1Repository;

    public IFeatSM1Repository FeatSM1Repository
    {
        get
        {
            if (Equals(_featSM1Repository, null))
            {
                _featSM1Repository = new FeatSM1Repository(_dbContext);
            }

            return _featSM1Repository;
        }
    }
}
