using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G62;

public class G62Handler : IFeatureHandler<G62Request, G62Response>
{
    private readonly IG62Repository _G62Repository;

    public G62Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _G62Repository = unitOfWork.Value.G62Repository;
    }

    public async Task<G62Response> ExecuteAsync(G62Request request, CancellationToken ct)
    {
        try
        {
            if (!await _G62Repository.IsFollowedCreator(request.UserId, request.CreatorId, ct))
            {
                return new G62Response
                {
                    IsSuccess = false,
                    StatusCode = G62ResponseStatusCode.INVALID_REQUEST
                };
            }

            return new G62Response
            {
                IsSuccess = await _G62Repository.UnfollowCreator(request.UserId, request.CreatorId, ct),
                StatusCode = G62ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G62Response
            {
                IsSuccess = false,
                StatusCode = G62ResponseStatusCode.FAILED
            };
        }
    }
}
