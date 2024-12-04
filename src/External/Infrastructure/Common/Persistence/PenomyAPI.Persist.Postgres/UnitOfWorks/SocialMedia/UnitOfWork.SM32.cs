using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM32Repository _featSM32Repository;

    public ISM32Repository FeatSM32Repository
    {
        get
        {
            if (Equals(_featSM32Repository, null)) _featSM32Repository = new SM32Repository(_dbContext);

            return _featSM32Repository;
        }
    }
}
