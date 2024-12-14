using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM17Repository _SM17Repository;

    public ISM17Repository SM17Repository
    {
        get
        {
            if (Equals(_SM17Repository, null))
                _SM17Repository = new SM17Repository(_dbContext);

            return _SM17Repository;
        }
    }
}
