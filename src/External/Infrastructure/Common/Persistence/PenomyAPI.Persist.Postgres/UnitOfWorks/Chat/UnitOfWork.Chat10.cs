using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public partial class UnitOfWork
{
    private IChat10Repository _chat10Repository;

    public IChat10Repository Chat10Repository
    {
        get
        {
            _chat10Repository ??= new Chat10Repository(_dbContext);

            return _chat10Repository;
        }
    }
}
