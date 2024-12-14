using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;
using PenomyAPI.Persist.Postgres.Repositories.Features.SystemManagement;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISys1Repository _Sys1Repository;

    public ISys1Repository Sys1Repository
    {
        get
        {
            if (Equals(_Sys1Repository, null))
            {
                _Sys1Repository = new Sys1Repository(_dbContext);
            }

            return _Sys1Repository;
        }
    }
}
