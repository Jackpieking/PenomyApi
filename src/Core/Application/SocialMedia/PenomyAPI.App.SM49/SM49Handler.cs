using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM49;

public class SM49Handler : IFeatureHandler<SM49Request, SM49Response>
{
    private readonly ISM49Repository _sm49Repository;

    public SM49Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm49Repository = unitOfWork.Value.SM49Repository;
    }

    public async Task<SM49Response> ExecuteAsync(SM49Request request, CancellationToken ct)
    {
        var response = new SM49Response();
        try
        {
            if (await _sm49Repository.IsFriendRequestExistsAsync(request.UserId, request.UserId, ct))
            {
                response.StatusCode = SM49ResponseStatusCode.REQUEST_NOT_FOUND;
                return response;
            }

            if (await _sm49Repository.IsUserFriendExistsAsync(request.UserId, request.FriendId, ct))
            {
                response.StatusCode = SM49ResponseStatusCode.ALREADY_FRIEND;
                return response;
            }

            IEnumerable<UserFriend> friends = new List<UserFriend>
            {
                new()
                {
                    FriendId = request.FriendId,
                    UserId = request.UserId,
                    StartedAt = DateTime.UtcNow
                },
                new()
                {
                    FriendId = request.UserId,
                    UserId = request.FriendId,
                    StartedAt = DateTime.UtcNow
                }
            };
            var result = await _sm49Repository.AcceptFriendRequestAsync(friends, ct);
            response.IsSuccess = result;
            response.StatusCode = result ? SM49ResponseStatusCode.SUCCESS : SM49ResponseStatusCode.FAILED;
        }
        catch (Exception)
        {
            response.StatusCode = SM49ResponseStatusCode.FAILED;
        }

        return response;
    }
}
