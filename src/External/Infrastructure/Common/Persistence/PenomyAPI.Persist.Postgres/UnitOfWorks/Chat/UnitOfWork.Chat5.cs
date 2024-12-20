using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IChat5Repository _featChat5Repository;

    public IChat5Repository Chat5Repository
    {
        get
        {
            if (Equals(_featChat5Repository, null)) _featChat5Repository = new Chat5Repository(_dbContext);

            return _featChat5Repository;
        }
    }
}
