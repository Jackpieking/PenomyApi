using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM40;

public class SM40Handler : IFeatureHandler<SM40Request, SM40Response>
{
    private readonly ISM40Repository _sm40Repository;

    public SM40Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm40Repository = unitOfWork.Value.SM40Repository;
    }

    public async Task<SM40Response> ExecuteAsync(SM40Request request, CancellationToken ct)
    {
        if (!await _sm40Repository.CheckMemberRoleAsync(request.GroupId, request.UserId, ct))
            return new SM40Response
            {
                IsSuccess = false,
                StatusCode = SM40ResponseStatusCode.FOBIDDEN,
            };

        var response = await _sm40Repository.ChangeGroupMemberRoleAsync(
            request.GroupId,
            request.MemberId,
            ct
        );

        if (response)
            return new SM40Response
            {
                IsSuccess = true,
                StatusCode = SM40ResponseStatusCode.SUCCESS,
            };

        return new SM40Response { IsSuccess = false, StatusCode = SM40ResponseStatusCode.FAILED };
    }
}
