using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

// Lưu ý namespace phải giống nhau.
namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.Art7
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt7Repository _art7Repository;

    public IArt7Repository Art7Repository
    {
        get
        {
            if (Equals(_art7Repository, null))
            {
                _art7Repository = new Art7Repository(_dbContext);
            }

            return _art7Repository;
        }
    }
}

