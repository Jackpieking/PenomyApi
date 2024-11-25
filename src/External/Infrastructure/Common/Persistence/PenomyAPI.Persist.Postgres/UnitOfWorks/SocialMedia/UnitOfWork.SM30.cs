using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM30Repository _featSM30Repository;

    public ISM30Repository FeatSM30Repository
    {
        get
        {
            if (Equals(_featSM30Repository, null)) _featSM30Repository = new SM30Repository(_dbContext, _userManager);

            return _featSM30Repository;
        }
    }
}
