using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM5;

public class SM5Handler : IFeatureHandler<SM5Request, SM5Response>
{
    private readonly ISM5Repository _SM5Repository;

    public SM5Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM5Repository = unitOfWork.Value.SM5Repository;
    }

    public async Task<SM5Response> ExecuteAsync(SM5Request request, CancellationToken ct)
    {
        var group = await _SM5Repository.GetGroupDetailAsync(request.UserId, request.GroupId);
        return new SM5Response
        {
            StatusCode = SM5ResponseStatusCode.SUCCESS,
            Group = group,
            IsSuccess = true,
        };
    }
}
