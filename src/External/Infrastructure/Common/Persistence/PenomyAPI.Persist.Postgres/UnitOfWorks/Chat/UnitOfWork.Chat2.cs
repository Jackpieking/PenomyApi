using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private IChat2Repository _featChat2Repository;

    public IChat2Repository Chat2Repository
    {
        get
        {
            if (Equals(_featChat2Repository, null)) _featChat2Repository = new Chat2Repository(_dbContext);

            return _featChat2Repository;
        }
    }
}
