using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Common;

public interface IDataSeedingRepository
{
    /// <summary>
    ///     Check if the current database has seeded the data or not
    ///     to process the data seeding operation.
    /// </summary>
    /// <returns>
    ///     Return <see langword="true"/> if the seed data has already existed.
    /// </returns>
    Task<bool> HasSeedDataAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Proceed to seed the pre-configured & required data into database
    ///     for the current system to operate properly.
    /// </summary>
    /// <returns>
    ///     Return <see langword="true"/> if the seeding operation is success.
    /// </returns>
    Task<bool> SeedDataAsync(CancellationToken cancellationToken);
}
