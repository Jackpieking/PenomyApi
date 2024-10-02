using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

// Lưu ý namespace phải giống nhau.
namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.Art4
/// </summary>
public sealed partial class UnitOfWork
{
    private IArt4Repository _art4Repository;

    public IArt4Repository Art4Repository
    {
        get
        {
            if (Equals(_art4Repository, null))
            {
                _art4Repository = new Art4Repository(_dbContext);
            }

            return _art4Repository;
        }
    }
}
