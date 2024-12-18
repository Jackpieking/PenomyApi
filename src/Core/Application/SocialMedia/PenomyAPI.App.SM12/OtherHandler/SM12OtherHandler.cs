using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM12.OtherHandler;

public class SM12OtherHandler : IFeatureHandler<SM12OtherRequest, SM12OtherResponse>
{
    private readonly ISM12Repository _sm12Repository;

    public SM12OtherHandler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _sm12Repository = unitOfWork.Value.FeatSM12Repository;
    }

    public async Task<SM12OtherResponse> ExecuteAsync(SM12OtherRequest request, CancellationToken ct)
    {
        var result = await _sm12Repository.GetUserFriendRequestsAsync(request.UserId, ct);
        return new SM12OtherResponse
        {
            StatusCode = Sm12OtherResponseStatusCode.SUCCESS,
            FriendRequest = result
        };
    }
}
