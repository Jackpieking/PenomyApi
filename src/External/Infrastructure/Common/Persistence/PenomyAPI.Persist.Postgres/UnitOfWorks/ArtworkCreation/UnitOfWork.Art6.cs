using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.Art6
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt6Repository _art6Repository;

    public IArt6Repository Art6Repository
    {
        get
        {
            if (Equals(_art6Repository, null))
            {
                _art6Repository = new Art6Repository(_dbContext);
            }

            return _art6Repository;
        }
    }
}
