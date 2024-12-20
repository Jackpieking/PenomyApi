using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art14Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt14Repository _art14Repository;

    public IArt14Repository Art14Repository
    {
        get
        {
            if (Equals(_art14Repository, null))
            {
                _art14Repository = new Art14Repository(_dbContext);
            }

            return _art14Repository;
        }
    }
}
