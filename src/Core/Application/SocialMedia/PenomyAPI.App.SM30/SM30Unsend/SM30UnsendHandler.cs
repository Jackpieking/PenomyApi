using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.SM30.SM30UnsendHandler;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM30Unsend.SM30UnsendUnsendHandler;

public class SM30UnsendHandler : IFeatureHandler<SM30UnsendRequest, SM30UnsendResponse>
{
    private readonly ISM30Repository _SM30UnsendRepository;

    public SM30UnsendHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM30UnsendRepository = unitOfWork.Value.FeatSM30Repository;
    }

    public async Task<SM30UnsendResponse> ExecuteAsync(SM30UnsendRequest request, CancellationToken ct)
    {
        var response = new SM30UnsendResponse();

        try
        {
            // Check if the user exists
            var userExists = await _SM30UnsendRepository.IsUserExistAsync(request.FriendId, ct);
            if (!userExists)
            {
                response.StatusCode = SM30UnsendResponseStatusCode.FAILED;
                return response;
            }

            // Check if they are already friends
            var alreadyFriend = await _SM30UnsendRepository.IsAlreadyFriendAsync(
                request.UserId,
                request.FriendId,
                ct
            );
            if (alreadyFriend)
            {
                response.StatusCode = SM30UnsendResponseStatusCode.FAILED;
                return response;
            }

            if (!await _SM30UnsendRepository.IsAlreadySendAsync(request.UserId, request.FriendId, ct))
            {
                response.StatusCode = SM30UnsendResponseStatusCode.NOT_SEND;
                return response;
            }

            // Create and send a friend request
            UserFriendRequest friendRequest =
                new()
                {
                    FriendId = request.FriendId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId,
                    RequestStatus = RequestStatus.Pending
                };

            var result = await _SM30UnsendRepository.UnSendFriendRequest(friendRequest, ct);
            response.StatusCode = result
                ? SM30UnsendResponseStatusCode.SUCCESS
                : SM30UnsendResponseStatusCode.FAILED;
        }
        catch
        {
            // Handle unexpected errors
            response.StatusCode = SM30UnsendResponseStatusCode.FAILED;
        }

        return response;
    }
}
