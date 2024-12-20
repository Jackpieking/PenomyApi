using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art17Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt17Repository _art17Repository;

    public IArt17Repository Art17Repository
    {
        get
        {
            if (Equals(_art17Repository, null))
            {
                _art17Repository = new Art17Repository(_dbContext);
            }

            return _art17Repository;
        }
    }
}
