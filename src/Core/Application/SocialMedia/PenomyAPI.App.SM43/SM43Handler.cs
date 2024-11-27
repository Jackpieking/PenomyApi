using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM43;

public class SM43Handler : IFeatureHandler<SM43Request, SM43Response>
{
    private readonly ISM43Repository _sm43Repository;

    public SM43Handler(Lazy<IUnitOfWork> unitOfWork, ISnowflakeIdGenerator snowflakeIdGenerator)
    {
        _sm43Repository = unitOfWork.Value.SM43Repository;
    }

    public async Task<SM43Response> ExecuteAsync(SM43Request request, CancellationToken ct)
    {
        var groupMember = new SocialGroupMember
        {
            GroupId = request.GroupId,
            MemberId = request.MemberId,
            JoinedAt = DateTime.UtcNow,
            RoleId = 2,
        };

        var memberId = await _sm43Repository.AcceptGroupJoinRequestAsync(
            groupMember,
            request.UserId
        );

        if (memberId == -1)
        {
            return new SM43Response
            {
                StatusCode = SM43ResponseStatusCode.FAILED,
                MemberId = -1,
                IsSuccess = false,
            };
        }

        return new SM43Response
        {
            StatusCode = SM43ResponseStatusCode.SUCCESS,
            MemberId = memberId,
            IsSuccess = true,
        };
    }
}
