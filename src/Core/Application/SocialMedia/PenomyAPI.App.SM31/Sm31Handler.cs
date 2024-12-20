using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM31;

public class Sm31Handler : IFeatureHandler<SM31Request, SM31Response>
{
    private readonly ISM31Repository _sm31Repository;

    public Sm31Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm31Repository = unitOfWork.Value.FeatSM31Repository;
    }

    public async Task<SM31Response> ExecuteAsync(SM31Request request, CancellationToken ct)
    {
        var response = new SM31Response();

        try
        {
            // Check if the user exists
            var userExists = await _sm31Repository.IsUserExistAsync(request.FriendId, ct);
            if (!userExists)
            {
                response.StatusCode = SM31ResponseStatusCode.USER_NOT_FOUND;
                return response;
            }

            // Check if they are already friends
            var alreadyFriend = await _sm31Repository.IsAlreadyFriendAsync(
                request.UserId,
                request.FriendId,
                ct
            );

            // Check if has sent friend request
            var hasFriendRequest = await _sm31Repository.HasFriendRequestAsync(
                request.UserId,
                request.FriendId,
                ct
            );

            if (!alreadyFriend && !hasFriendRequest)
            {
                response.StatusCode = SM31ResponseStatusCode.IS_NOT_FRIEND;
                return response;
            }

            IEnumerable<UserFriend> friends = new List<UserFriend>
            {
                new()
                {
                    FriendId = request.FriendId,
                    UserId = request.UserId,
                    StartedAt = DateTime.UtcNow
                }
            };
            var result = await _sm31Repository.UnfriendAsync(friends, ct);
            response.StatusCode = result
                ? SM31ResponseStatusCode.SUCCESS
                : SM31ResponseStatusCode.FAILED;
        }
        catch
        {
            // Handle unexpected errors
            response.StatusCode = SM31ResponseStatusCode.FAILED;
        }

        return response;
    }
}
