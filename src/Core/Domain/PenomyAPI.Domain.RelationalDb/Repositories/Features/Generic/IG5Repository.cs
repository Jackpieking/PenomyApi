using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<G5ComicDetailReadModel> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken ct = default
    );

    Task<G5UserArtworkPreferenceReadModel> GetUserArtworkPreferenceAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken);

    Task<G5UserArtworkPreferenceReadModel> GetGuestArtworkPreferenceAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken);

    Task<bool> IsCreatorProfileExistedAsync(
        long creatorId,
        CancellationToken cancellationToken);

    Task<G5CreatorProfileReadModel> GetCreatorProfileBasedOnUserIdAsync(
        long userId,
        long creatorId,
        CancellationToken cancellationToken);
}
