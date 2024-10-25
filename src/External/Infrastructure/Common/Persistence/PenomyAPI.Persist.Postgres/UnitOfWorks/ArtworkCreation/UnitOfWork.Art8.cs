using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

// Lưu ý namespace phải giống nhau.
namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.Art8
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt8Repository _art8Repository;

    public IArt8Repository Art8Repository
    {
        get
        {
            if (Equals(_art8Repository, null))
            {
                _art8Repository = new Art8Repository(_dbContext);
            }

            return _art8Repository;
        }
    }
}

