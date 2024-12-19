using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;

public class G5UserPreferenceHandler
    : IFeatureHandler<G5UserPreferenceRequest, G5UserPreferenceResponse>
{
    private IG5Repository _g5Repository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G5UserPreferenceHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G5UserPreferenceResponse> ExecuteAsync(
        G5UserPreferenceRequest request,
        CancellationToken ct)
    {
        _g5Repository = _unitOfWork.Value.FeatG5Repository;

        if (request.IsGuest)
        {
            var guestPreference = await _g5Repository.GetGuestArtworkPreferenceAsync(
                request.GuestId,
                request.ArtworkId,
                ct);

            return new()
            {
                UserArtworkPreference = guestPreference,
            };
        }

        var userPreference = await _g5Repository.GetUserArtworkPreferenceAsync(
            request.UserId,
            request.ArtworkId,
            ct);

        return new()
        {
            UserArtworkPreference = userPreference,
        };
    }
}
