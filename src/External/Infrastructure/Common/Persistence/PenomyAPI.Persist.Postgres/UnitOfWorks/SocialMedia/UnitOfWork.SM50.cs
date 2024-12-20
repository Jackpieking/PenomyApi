using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM50Repository _SM50Repository;

    public ISM50Repository SM50Repository
    {
        get
        {
            _SM50Repository ??= new SM50Repository(_dbContext);

            return _SM50Repository;
        }
    }
}
