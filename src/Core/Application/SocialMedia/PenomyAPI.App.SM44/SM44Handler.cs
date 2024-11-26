using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM44;

public class SM44Handler : IFeatureHandler<SM44Request, SM44Response>
{
    private readonly ISM44Repository _sm44Repository;

    public SM44Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm44Repository = unitOfWork.Value.SM44Repository;
    }

    public async Task<SM44Response> ExecuteAsync(SM44Request request, CancellationToken ct)
    {
        var response = new SM44Response();
        try
        {
            if (request.UserId != 0)
            {
                response.IsSuccess = await _sm44Repository.CreateGroupJoinRequestAsync(
                    request.GroupId,
                    request.UserId
                );
            }

            response.StatusCode = SM44ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.IsSuccess = false;
            response.StatusCode = SM44ResponseStatusCode.FAILED;
        }

        return response;
    }
}
