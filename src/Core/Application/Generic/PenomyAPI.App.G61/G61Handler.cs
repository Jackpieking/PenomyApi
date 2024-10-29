using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G61;

public class G61Handler : IFeatureHandler<G61Request, G61Response>
{
    private readonly IG61Repository _g61Repository;

    public G61Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g61Repository = unitOfWork.Value.G61Repository;
    }

    public async Task<G61Response> ExecuteAsync(G61Request request, CancellationToken ct)
    {
        try
        {
            if (!await _g61Repository.IsCreator(request.CreatorId, ct) ||
                await _g61Repository.IsFollowedCreator(request.UserId, request.CreatorId, ct))
            {
                return new G61Response
                {
                    IsSuccess = false,
                    StatusCode = G61ResponseStatusCode.INVALID_REQUEST
                };
            }

            return new G61Response
            {
                IsSuccess = await _g61Repository.FollowCreator(request.UserId, request.CreatorId, ct),
                StatusCode = G61ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new G61Response
            {
                IsSuccess = false,
                StatusCode = G61ResponseStatusCode.FAILED
            };
        }
    }
}
