using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM31Repository _featSM31Repository;

    public ISM31Repository FeatSM31Repository
    {
        get
        {
            if (Equals(_featSM31Repository, null)) _featSM31Repository = new SM31Repository(_dbContext, _userManager);

            return _featSM31Repository;
        }
    }
}
