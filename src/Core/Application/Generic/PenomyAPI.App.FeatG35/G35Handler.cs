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
        // Check if the current user is already registered as creator or not.
        var isCreator = await _repository.IsUserRegisteredAsCreatorByIdAsync(
            request.UserId,
            ct);

        var userProfile = await _repository.GetUserProfileByIdAsync(
            request.UserId,
            isCreator,
            ct);

        return new G35Response
        {
            UserProfile = userProfile
        };
    }
}
