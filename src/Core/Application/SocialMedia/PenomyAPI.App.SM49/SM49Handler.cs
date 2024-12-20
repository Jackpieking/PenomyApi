using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM49;

public class SM49Handler : IFeatureHandler<SM49Request, SM49Response>
{
    private readonly IFeatChat1Repository _Chat1Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly ISM49Repository _sm49Repository;

    public SM49Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _sm49Repository = unitOfWork.Value.SM49Repository;
        _Chat1Repository = unitOfWork.Value.FeatChat1Repository;
        _idGenerator = idGenerator;
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
                    FriendId = request.UserId,
                    UserId = request.FriendId,
                    StartedAt = DateTime.UtcNow
                }
            };
            var result = await _sm49Repository.AcceptFriendRequestAsync(friends, ct);
            response.IsSuccess = result;
            var success = false;
            if (response.IsSuccess)
            {
                var socialGroup = new ChatGroup
                {
                    Id = _idGenerator.Value.Get(),
                    GroupName = $"Nhóm chat của {request.UserId} và {request.FriendId}",
                    IsPublic = false,
                    CoverPhotoUrl = string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId,
                    ChatGroupType = ChatGroupType.Friend,
                    TotalMembers = 2
                };
                var chatOwner = new ChatGroupMember
                {
                    MemberId = request.UserId,
                    ChatGroupId = socialGroup.Id,
                    RoleId = UserRoles.GroupManager.Id
                };
                var chatMember = new ChatGroupMember
                {
                    MemberId = request.FriendId,
                    ChatGroupId = socialGroup.Id,
                    RoleId = UserRoles.GroupManager.Id
                };
                success = await _Chat1Repository.CreateGroupAsync(socialGroup, chatOwner, ct);
            }

            response.StatusCode = success ? SM49ResponseStatusCode.SUCCESS : SM49ResponseStatusCode.FAILED;
        }
        catch (Exception)
        {
            response.StatusCode = SM49ResponseStatusCode.FAILED;
        }

        return response;
    }
}
