using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art3Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt3Repository _art3Repository;

    public IArt3Repository Art3Repository
    {
        get
        {
            if (Equals(_art3Repository, null))
            {
                _art3Repository = new Art3Repository(_dbContext);
            }

            return _art3Repository;
        }
    }
}
