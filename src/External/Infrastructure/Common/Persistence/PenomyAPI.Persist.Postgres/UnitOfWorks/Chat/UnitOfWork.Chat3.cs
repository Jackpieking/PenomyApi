using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IChat3Repository _featChat3Repository;

    public IChat3Repository Chat3Repository
    {
        get
        {
            if (Equals(_featChat3Repository, null)) _featChat3Repository = new Chat3Repository(_dbContext);

            return _featChat3Repository;
        }
    }
}
