using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM41Repository _SM41Repository;

    public ISM41Repository SM41Repository
    {
        get
        {
            _SM41Repository ??= new SM41Repository(_dbContext);

            return _SM41Repository;
        }
    }
}
