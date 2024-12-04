using PenomyAPI.App.Common;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM6;

public class SM6Handler : IFeatureHandler<SM6Request, SM6Response>
{
    private readonly ISM6Repository _SM6Repository;
    private readonly INotificationHub _notification;

    public SM6Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<INotificationHub> notification)
    {
        _SM6Repository = unitOfWork.Value.SM6Repository;
        _notification = notification.Value;
    }

    public async Task<SM6Response> ExecuteAsync(SM6Request request, CancellationToken ct)
    {
        try
        {
            if (await _SM6Repository.CheckGroupExists(request.GroupId, ct) &&
                await _SM6Repository.CheckUserJoinedGroupAsync(request.UserId, request.GroupId, ct))
            {
                return new SM6Response
                {
                    IsSuccess = false,
                    StatusCode = SM6ResponseStatusCode.INVALID_REQUEST
                };
            }

            bool isAddSuccess = await _SM6Repository
                .AddUserJoinRequestByUserIdAndGroupIdAsync(
                    request.UserId,
                    request.GroupId,
                    ct
                    );

            if (isAddSuccess)
            {
                await _notification.SendNotifToClient(request.UserId.ToString());

                return new SM6Response
                {
                    IsSuccess = isAddSuccess,
                    StatusCode = SM6ResponseStatusCode.SUCCESS
                };
            }
        }
        catch
        {
            return new SM6Response
            {
                IsSuccess = false,
                StatusCode = SM6ResponseStatusCode.FAILED
            };
        }

        return new SM6Response
        {
            IsSuccess = false,
            StatusCode = SM6ResponseStatusCode.FAILED
        };
    }
}
