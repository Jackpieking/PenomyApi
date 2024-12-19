using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art22Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt22Repository _art22Repository;

    public IArt22Repository Art22Repository
    {
        get
        {
            if (Equals(_art22Repository, null))
            {
                _art22Repository = new Art22Repository(_dbContext);
            }

            return _art22Repository;
        }
    }
}
