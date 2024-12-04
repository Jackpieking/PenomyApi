using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM24Repository _SM24Repository;

    public ISM24Repository SM24Repository
    {
        get
        {
            _SM24Repository ??= new SM24Repository(_dbContext);

            return _SM24Repository;
        }
    }
}
