using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM15Repository _featSM15Repository;

    public ISM15Repository FeatSM15Repository
    {
        get
        {
            if (Equals(_featSM15Repository, null))
                _featSM15Repository = new SM15Repository(_dbContext);

            return _featSM15Repository;
        }
    }
}
