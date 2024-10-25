using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.Art5
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt5Repository _art5Repository;

    public IArt5Repository Art5Repository
    {
        get
        {
            if (Equals(_art5Repository, null))
            {
                _art5Repository = new Art5Repository(_dbContext);
            }

            return _art5Repository;
        }
    }
}
