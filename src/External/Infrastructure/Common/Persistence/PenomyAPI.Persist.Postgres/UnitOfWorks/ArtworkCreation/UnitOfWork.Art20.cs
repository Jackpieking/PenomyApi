using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art1Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt20Repository _art20Repository;

    public IArt20Repository Art20Repository
    {
        get
        {
            if (Equals(_art20Repository, null))
            {
                _art20Repository = new Art20Repository(_dbContext);
            }

            return _art20Repository;
        }
    }
}
