using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Persist.Postgres.Repositories.Common;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for Creator Repository
/// </summary>
public sealed partial class UnitOfWork
{
    private ICreatorRepository _creatorRepository;

    public ICreatorRepository CreatorRepository
    {
        get
        {
            if (Equals(_creatorRepository, null))
            {
                _creatorRepository = new CreatorRepository(_dbContext);
            }

            return _creatorRepository;
        }
    }
}
