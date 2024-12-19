using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Art15Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt15Repository _art15Repository;

    public IArt15Repository Art15Repository
    {
        get
        {
            if (Equals(_art15Repository, null))
            {
                _art15Repository = new Art15Repository(_dbContext);
            }

            return _art15Repository;
        }
    }
}
