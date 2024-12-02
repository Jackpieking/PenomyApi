using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM46Repository _SM46Repository;

    public ISM46Repository SM46Repository
    {
        get
        {
            _SM46Repository ??= new SM46Repository(_dbContext);

            return _SM46Repository;
        }
    }
}
