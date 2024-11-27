using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM44Repository _SM44Repository;

    public ISM44Repository SM44Repository
    {
        get
        {
            _SM44Repository ??= new SM44Repository(_dbContext);

            return _SM44Repository;
        }
    }
}
