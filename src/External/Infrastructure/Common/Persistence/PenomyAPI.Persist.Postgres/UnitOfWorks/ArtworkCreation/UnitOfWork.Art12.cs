using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art12Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt12Repository _art12Repository;

    public IArt12Repository Art12Repository
    {
        get
        {
            if (Equals(_art12Repository, null))
            {
                _art12Repository = new Art12Repository(_dbContext);
            }

            return _art12Repository;
        }
    }
}
