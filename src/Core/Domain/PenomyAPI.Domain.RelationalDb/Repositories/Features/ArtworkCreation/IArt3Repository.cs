using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt3Repository
{
    Task<Art3CheckDeletedItemReadModel> CheckCurrentCreatorDeletedItemsAsync(
        long creatorId,
        CancellationToken cancellationToken);

    Task<bool> HasAnyDeletedItemsWithArtworTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    Task<List<Art3DeletedArtworkDetailReadModel>> GetAllDeletedArtworksByTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    Task<bool> AreArtworksTemporarilyRemovedAsync(
        IEnumerable<long> artworkIds,
        long creatorId,
        CancellationToken cancellationToken);

    Task<bool> PermanentlyRemoveArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken);

    Task<bool> RestoreDeletedArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken);

    Task<bool> RestoreAllDeletedItemsByArtworkTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    Task<bool> PermanentlyRemoveAllDeletedItemsByArtworkTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);
}
