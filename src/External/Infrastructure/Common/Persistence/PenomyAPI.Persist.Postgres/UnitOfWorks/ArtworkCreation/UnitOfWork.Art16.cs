using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art16Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt16Repository _art16Repository;

    public IArt16Repository Art16Repository
    {
        get
        {
            if (Equals(_art16Repository, null))
            {
                _art16Repository = new Art16Repository(_dbContext);
            }

            return _art16Repository;
        }
    }
}
