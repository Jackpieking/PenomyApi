using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM45;

public class SM45Handler : IFeatureHandler<SM45Request, SM45Response>
{
    private readonly ISM45Repository _sm45Repository;

    public SM45Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm45Repository = unitOfWork.Value.SM45Repository;
    }

    public async Task<SM45Response> ExecuteAsync(SM45Request request, CancellationToken ct)
    {
        var response = new SM45Response();
        try
        {
            response.IsSuccess = await _sm45Repository.CancelJoinGroupRequestAsync(
                request.GroupId,
                long.Parse(request.GetUserId()),
                ct
            );

            response.StatusCode = SM45ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.IsSuccess = false;
            response.StatusCode = SM45ResponseStatusCode.FAILED;
        }

        return response;
    }
}
