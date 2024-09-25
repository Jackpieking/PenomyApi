using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

// Lưu ý namespace phải giống nhau.
namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork.FeatArt4
/// </summary>
public sealed partial class UnitOfWork
{
    private IFeatArt4Repository _FeatArt4Repository;

    public IFeatArt4Repository FeatArt4Repository
    {
        get
        {
            if (Equals(_FeatArt4Repository, null))
            {
                _FeatArt4Repository = new FeatArt4Repository(_dbContext);
            }

            return _FeatArt4Repository;
        }
    }
}
