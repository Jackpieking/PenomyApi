using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM39Repository _SM39Repository;

    public ISM39Repository SM39Repository
    {
        get
        {
            _SM39Repository ??= new SM39Repository(_dbContext);

            return _SM39Repository;
        }
    }
}
