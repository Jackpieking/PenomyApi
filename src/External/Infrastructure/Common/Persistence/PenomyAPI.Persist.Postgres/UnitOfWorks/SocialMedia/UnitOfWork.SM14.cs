using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM14Repository _featSM14Repository;

    public ISM14Repository FeatSM14Repository
    {
        get
        {
            if (Equals(_featSM14Repository, null))
                _featSM14Repository = new SM14Repository(_dbContext);

            return _featSM14Repository;
        }
    }
}
