using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM40Repository _SM40Repository;

    public ISM40Repository SM40Repository
    {
        get
        {
            _SM40Repository ??= new SM40Repository(_dbContext);

            return _SM40Repository;
        }
    }
}
