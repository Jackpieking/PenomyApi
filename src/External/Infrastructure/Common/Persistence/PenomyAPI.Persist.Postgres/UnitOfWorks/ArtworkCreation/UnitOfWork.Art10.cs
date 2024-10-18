using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art1Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt10Repository _art10Repository;

    public IArt10Repository Art10Repository
    {
        get
        {
            if (Equals(_art10Repository, null))
            {
                _art10Repository = new Art10Repository(_dbContext);
            }

            return _art10Repository;
        }
    }
}
