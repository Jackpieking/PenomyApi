using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;

public sealed class G35GetCreatorProfileHandler
    : IFeatureHandler<G35GetCreatorProfileRequest, G35GetCreatorProfileResponse>
{
    private readonly IG35Repository _g35repository;

    public G35GetCreatorProfileHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g35repository = unitOfWork.Value.G35Repository;
    }

    public async Task<G35GetCreatorProfileResponse> ExecuteAsync(
        G35GetCreatorProfileRequest request,
        CancellationToken ct)
    {
        // Check if the creator id is existed or not.
        var isCreatoIdExisted = await _g35repository.IsCreatorIdExistedAsync(
            request.CreatorId,
            ct);

        if (!isCreatoIdExisted)
        {
            return G35GetCreatorProfileResponse.CREATOR_ID_NOT_FOUND;
        }

        var creatorProfile = await _g35repository.GetCreatorProfileByIdAsync(
            request.CreatorId,
            ct);

        return G35GetCreatorProfileResponse.SUCCESS(creatorProfile);
    }
}
