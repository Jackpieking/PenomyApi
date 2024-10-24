using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt8Repository
{
    /// <summary>
    ///     Check if the artwork with specified input <paramref name="artworkId"/>
    ///     is temporarily removed or not.
    /// </summary>
    /// <param name="artworkId">
    ///     The id of the artwork to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The checking result (<see langword="bool"/>).
    /// </returns>
    Task<bool> IsArtworkTemporarilyRemovedByIdAsync(
        long artworkId,
        CancellationToken cancellationToken);

    Task<bool> TemporarilyRemoveArtworkByIdAsync(
        long artworkId,
        long removedBy,
        DateTime removedAt,
        CancellationToken cancellationToken);
}
