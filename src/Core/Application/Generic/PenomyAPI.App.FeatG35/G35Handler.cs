using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG35;

public sealed class G35Handler : IFeatureHandler<G35Request, G35Response>
{
    private readonly IG35Repository _repository;

    public G35Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.G35Repository;
    }

    public async Task<G35Response> ExecuteAsync(G35Request request, CancellationToken ct)
    {
        // Check if the input user id is existed or not.
        var isUserExisted = await _repository.IsUserIdExistedAsync(request.UserId, ct);

        if (!isUserExisted)
        {
            return G35Response.USER_ID_NOT_FOUND;
        }

        // Check if the current user has already registered
        // as creator or not to fetch additional data.
        var hasUserRegisteredAsCreator = await _repository.IsCreatorIdExistedAsync(request.UserId, ct);

        if (hasUserRegisteredAsCreator)
        {
            var userProfileWithCreatorAdditionalInfo = await _repository.GetUserProfileAsCreatorByIdAsync(
                    request.UserId,
                    request.IsProfileOwner,
                    ct);

            return G35Response.SUCCESS(userProfileWithCreatorAdditionalInfo, request.IsProfileOwner);
        }

        // Otherwise just fetch the related data from user profile.
        var userProfile = await _repository.GetUserProfileByIdAsync(
            request.UserId,
            request.IsProfileOwner,
            ct);

        return G35Response.SUCCESS(userProfile, request.IsProfileOwner);
    }
}
