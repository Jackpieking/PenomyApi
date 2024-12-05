using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM25Repository _SM25Repository;

    public ISM25Repository SM25Repository
    {
        get
        {
            _SM25Repository ??= new SM25Repository(_dbContext);

            return _SM25Repository;
        }
    }
}
