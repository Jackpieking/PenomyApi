using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM32;

public class SM32Handler : IFeatureHandler<SM32Request, SM32Response>
{
    private readonly ISM32Repository _sm32Repository;

    public SM32Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm32Repository = unitOfWork.Value.FeatSM32Repository;
    }

    public async Task<SM32Response> ExecuteAsync(SM32Request request, CancellationToken ct)
    {
        var response = new SM32Response();
        response.FriendIds = await _sm32Repository.GetAllUserFriendsAsync(
            request.UserId,
            request.FriendId,
            ct
        );
        response.SendToMeFriendRequestIds = await _sm32Repository.GetUserFriendRequestAsync(
            request.UserId,
            ct
        );
        response.SendByMeFriendRequestIds = await _sm32Repository.GetAllUserFriendRequestAsync(
            request.UserId,
            ct
        );
        response.UserProfiles = await _sm32Repository.GetAllUserProfilesAsync(request.UserId, ct);

        response.StatusCode = SM32ResponseStatusCode.SUCCESS;
        response.IsSuccess = true;

        return response;
    }
}
