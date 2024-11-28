using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM39;

public class SM39Handler : IFeatureHandler<SM39Request, SM39Response>
{
    private readonly ISM39Repository _sm39Repository;

    public SM39Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm39Repository = unitOfWork.Value.SM39Repository;
    }

    public async Task<SM39Response> ExecuteAsync(SM39Request request, CancellationToken ct)
    {
        var response = await _sm39Repository.GetGroupMemberAsync(long.Parse(request.GroupId), ct);

        return new SM39Response
        {
            IsSuccess = true,
            StatusCode = SM39ResponseStatusCode.SUCCESS,
            Members = response,
        };
    }
}
