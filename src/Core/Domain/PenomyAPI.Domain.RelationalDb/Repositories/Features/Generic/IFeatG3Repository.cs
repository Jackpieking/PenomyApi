using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IFeatG3Repository
{
    Task<List<RecentlyUpdatedComicReadModel>> GetRecentlyUpdatedArtworksAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken);
}
