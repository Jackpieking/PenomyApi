using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG4;

public class G4Handler : IFeatureHandler<G4Request, G4Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IG4Repository _g4Repository;

    public G4Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
        _g4Repository = _unitOfWork.G4Repository;
    }

    public async Task<G4Response> ExecuteAsync(G4Request request, CancellationToken ct)
    {
        try
        {
            if (request.ForSignedInUser)
            {
                return await GetRecommendedComicsForUserAsync(request.UserId, ct);
            }

            // Check if the guest with specified id has viewed
            // any artwork or not to serve properly.
            var isCurrentGuestHasViewHistory = await _g4Repository.IsCurrentGuestHasViewHistoryAsync(
                request.GuestId,
                ct);

            if (isCurrentGuestHasViewHistory)
            {
                return await GetRecommendedComicsForGuestAsync(request.GuestId, ct);
            }

            // Otherwise, serve this client as a first time visit guest.
            return await GetRecommendedComicsForNewGuestAsync(ct);
        }
        catch
        {
            return new G4Response
            {
                StatusCode = G4ResponseStatusCode.DATABASE_ERROR
            };
        }
    }

    #region Private Methods
    private async Task<G4Response> GetRecommendedComicsForUserAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        List<RecommendedComicByCategory> recommendedArtworkByCategory = await _g4Repository
            .GetRecommendedComicsForUserAsync(
                userId,
                cancellationToken);

        return new()
        {
            RecommendedArtworkByCategories = recommendedArtworkByCategory,
            StatusCode = G4ResponseStatusCode.SUCCESS,
        };
    }

    private async Task<G4Response> GetRecommendedComicsForGuestAsync(
        long guestId,
        CancellationToken cancellationToken)
    {
        List<RecommendedComicByCategory> recommendedArtworkByCategory = await _g4Repository
            .GetRecommendedComicsForGuestAsync(
                guestId,
                cancellationToken);

        return new()
        {
            RecommendedArtworkByCategories = recommendedArtworkByCategory,
            StatusCode = G4ResponseStatusCode.SUCCESS,
        };
    }

    private async Task<G4Response> GetRecommendedComicsForNewGuestAsync(
        CancellationToken cancellationToken)
    {
        List<RecommendedComicByCategory> recommendedArtworkByCategory = await _g4Repository
            .GetRecommendedComicsForNewGuestAsync(cancellationToken);

        return new()
        {
            RecommendedArtworkByCategories = recommendedArtworkByCategory,
            StatusCode = G4ResponseStatusCode.SUCCESS,
        };
    }
    #endregion
}
