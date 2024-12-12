using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM42;

public class SM42Handler : IFeatureHandler<SM42Request, SM42Response>
{
    private readonly ISM42Repository _sm42Repository;

    public SM42Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm42Repository = unitOfWork.Value.SM42Repository;
    }

    public async Task<SM42Response> ExecuteAsync(SM42Request request, CancellationToken ct)
    {
        var response = new SM42Response();
        try
        {
            response.RequestList = await _sm42Repository.GetGroupJoinRequestAsync(request.GroupId);
            response.StatusCode = SM42ResponseStatusCode.SUCCESS;
            response.IsSuccess = true;
        }
        catch
        {
            response.IsSuccess = false;
            response.StatusCode = SM42ResponseStatusCode.FAILED;
        }

        return response;
    }
}
