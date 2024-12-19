using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art1Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt1Repository _art1Repository;

    public IArt1Repository Art1Repository
    {
        get
        {
            if (Equals(_art1Repository, null))
            {
                _art1Repository = new Art1Repository(_dbContext);
            }

            return _art1Repository;
        }
    }
}
