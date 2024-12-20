using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM6.OtherHandlers.JoinGroup;

public class SM6JoinGroupHandler : IFeatureHandler<SM6JoinGroupRequest, SM6JoinGroupResponse>
{
    private readonly ISM6Repository _SM6Repository;

    public SM6JoinGroupHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM6Repository = unitOfWork.Value.SM6Repository;
    }

    public async Task<SM6JoinGroupResponse> ExecuteAsync(SM6JoinGroupRequest request, CancellationToken ct)
    {
        try
        {
            return new SM6JoinGroupResponse
            {
                IsSuccess = await _SM6Repository
                .AddUserToGroupByUserIdAndGroupIdAsync(
                    request.UserId,
                    request.GroupId,
                    ct
                    ),
                StatusCode = SM6ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new SM6JoinGroupResponse
            {
                IsSuccess = false,
                StatusCode = SM6ResponseStatusCode.FAILED
            };
        }
    }
}
