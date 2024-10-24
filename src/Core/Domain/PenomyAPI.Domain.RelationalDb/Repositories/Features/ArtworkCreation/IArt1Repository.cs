using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt1;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt1Repository
{
    /// <summary>
    ///     Get the total numbers of artwork by the specified
    ///     <paramref name="artworkType"/> and <paramref name="creatorId"/>.
    /// </summary>
    /// <param name="artworkType">
    ///     The type of artwork to take.
    /// </param>
    /// <param name="creatorId">
    ///     The id of the creator who creates these artworks.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The total numbers of artwork.
    /// </returns>
    Task<int> GetTotalOfArtworksByTypeAndCreatorIdAsync(
        ArtworkType artworkType,
        long creatorId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get all the artworks by the specified <paramref name="creatorId"/>
    ///     and <paramref name="artworkType"/> using pagination.
    /// </summary>
    /// <remarks>
    ///     This method uses offset-based pagination to retrieve data.
    /// </remarks>
    /// <param name="artworkType">
    ///     The type of artwork to take.
    /// </param>
    /// <param name="creatorId">
    ///     The id of the creator who creates these artworks.
    /// </param>
    /// <param name="pageNumber">
    ///     The current page number to navigate.
    /// </param>
    /// <param name="pageSize">
    ///     The size of the page to take the artworks.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The list of artworks that 
    /// </returns>
    Task<List<Artwork>> GetArtworksByTypeAndCreatorIdWithPaginationAsync(
        ArtworkType artworkType,
        long creatorId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<OverviewStatisticReadModel> GetOverviewStatisticByCreatorIdAsync(
        long creatorId,
        CancellationToken cancellationToken);
}
