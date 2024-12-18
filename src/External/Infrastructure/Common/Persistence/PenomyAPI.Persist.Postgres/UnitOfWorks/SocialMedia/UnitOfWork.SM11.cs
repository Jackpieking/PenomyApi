using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM11Repository _featSM11Repository;

    public ISM11Repository FeatSM11Repository
    {
        get
        {
            if (Equals(_featSM11Repository, null))
                _featSM11Repository = new SM11Repository(_dbContext);

            return _featSM11Repository;
        }
    }
}
