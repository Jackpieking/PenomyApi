using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG37;

public class G37Handler : IFeatureHandler<G37Request, G37Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IG37Repository _g37Repository;

    public G37Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G37Response> ExecuteAsync(G37Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;

        var hasUserAlreadyBecomeCreator = await unitOfWork.CreatorRepository.HasUserAlreadyBecomeCreatorAsync(
            request.UserId,
            ct);

        if (hasUserAlreadyBecomeCreator)
        {
            return G37Response.USER_HAS_ALREADY_BECOME_CREATOR;
        }

        // Check the register result to return correct response.
        _g37Repository = unitOfWork.G37Repository;

        var registerResult = await _g37Repository.RegisterUserAsCreatorAsync(
            request.UserId,
            ct);

        if (registerResult)
        {
            return G37Response.SUCCESS;
        }

        return G37Response.DATABASE_ERROR;
    }
}
