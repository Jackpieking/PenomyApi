using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;

public class G5CreatorProfileDetailHandler
    : IFeatureHandler<G5CreatorProfileDetailRequest, G5CreatorProfileDetailResponse>
{
    private IG5Repository _g5Repository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G5CreatorProfileDetailHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G5CreatorProfileDetailResponse> ExecuteAsync(
        G5CreatorProfileDetailRequest request,
        CancellationToken ct)
    {
        _g5Repository = _unitOfWork.Value.FeatG5Repository;

        var isCreatorExisted = await _g5Repository.IsCreatorProfileExistedAsync(
            request.CreatorId,
            ct);

        if (!isCreatorExisted)
        {
            return G5CreatorProfileDetailResponse.CREATOR_NOT_FOUND;
        }

        var creatorProfile = await _g5Repository.GetCreatorProfileBasedOnUserIdAsync(
            request.CreatorId,
            request.CreatorId,
            ct);

        return new()
        {
            AppCode = G5CreatorProfileResponseAppCode.SUCCESS,
            CreatorProfile = creatorProfile,
        };
    }
}
