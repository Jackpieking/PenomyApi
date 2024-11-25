using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM30;

public class SM30Handler : IFeatureHandler<SM30Request, SM30Response>
{
    private readonly ISM30Repository _sm30Repository;

    public SM30Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _sm30Repository = unitOfWork.Value.FeatSM30Repository;
    }

    public async Task<SM30Response> ExecuteAsync(SM30Request request, CancellationToken ct)
    {
        var response = new SM30Response();

        try
        {
            // Check if the user exists
            var userExists = await _sm30Repository.IsUserExistAsync(request.FriendId, ct);
            if (!userExists)
            {
                response.StatusCode = SM30ResponseStatusCode.USER_NOT_FOUND;
                return response;
            }

            // Check if they are already friends
            var alreadyFriend = await _sm30Repository.IsAlreadyFriendAsync(request.UserId, request.FriendId, ct);
            if (alreadyFriend)
            {
                response.StatusCode = SM30ResponseStatusCode.ALREADY_FRIEND;
                return response;
            }

            // Create and send a friend request
            UserFriendRequest friendRequest = new()
            {
                FriendId = request.FriendId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId,
                RequestStatus = RequestStatus.Accepted
            };

            var result = await _sm30Repository.SendFriendRequest(friendRequest, ct);
            response.StatusCode = result ? SM30ResponseStatusCode.SUCCESS : SM30ResponseStatusCode.FAILED;
        }
        catch
        {
            // Handle unexpected errors
            response.StatusCode = SM30ResponseStatusCode.FAILED;
        }

        return response;
    }
}
