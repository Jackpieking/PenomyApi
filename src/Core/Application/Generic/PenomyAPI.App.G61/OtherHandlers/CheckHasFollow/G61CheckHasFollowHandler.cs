using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G61.OtherHandlers.CheckHasFollow;

public sealed class G61CheckHasFollowHandler
    : IFeatureHandler<G61CheckHasFollowRequest, G61CheckHasFollowResponse>
{
    private readonly IG61Repository _g61Repository;

    public G61CheckHasFollowHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _g61Repository = unitOfWork.Value.G61Repository;
    }

    public async Task<G61CheckHasFollowResponse> ExecuteAsync(
        G61CheckHasFollowRequest request,
        CancellationToken ct)
    {
        try
        {
            var hasUserFollowed = await _g61Repository.IsFollowedCreator(request.UserId, request.CreatorId, ct);

            return new()
            {
                HasFollowed = hasUserFollowed,
                StatusCode = G61ResponseStatusCode.SUCCESS,
            };
        }
        catch
        {
            return G61CheckHasFollowResponse.FAILED;
        }
    }
}
