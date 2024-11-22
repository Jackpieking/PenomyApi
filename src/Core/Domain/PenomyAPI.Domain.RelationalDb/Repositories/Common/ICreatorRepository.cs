using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Common;

public interface ICreatorRepository
{
    /// <summary>
    ///     Check if the user with specified id
    ///     has already registered as a creator or not.
    /// </summary>
    /// <param name="userId">
    ///     The userId to check.
    /// </param>
    public Task<bool> HasUserAlreadyBecomeCreatorAsync(
        long userId,
        CancellationToken cancellationToken);
}
