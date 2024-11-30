using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Persist.Postgres.Repositories.Common;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     The partial of UnitOfWork for DataSeedingRepository
/// </summary>
public sealed partial class UnitOfWork
{
    private IDataSeedingRepository _DataSeedingRepository;

    public IDataSeedingRepository DataSeedingRepository
    {
        get
        {
            if (Equals(_DataSeedingRepository, null))
            {
                _DataSeedingRepository = new DataSeedingRepository(_dbContext);
            }

            return _DataSeedingRepository;
        }
    }
}