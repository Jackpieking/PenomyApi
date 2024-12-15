using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM23;

public class SM23Handler : IFeatureHandler<SM23Request, SM23Response>
{
    private readonly ISM23Repository _sm23Repository;

    public SM23Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm23Repository = unitOfWork.Value.SM23Repository;
    }

    public async Task<SM23Response> ExecuteAsync(SM23Request request, CancellationToken ct)
    {
        var response = new SM23Response();
        try
        {
            if (!request.IsGroupPost)
            {
                var result = await _sm23Repository.GetUserPostCommentsAsync(
                    request.PostId,
                    request.UserId,
                    ct
                );
                response.UserPostComments = result;
                response.StatusCode = SM23ResponseStatusCode.SUCCESS;
                response.IsSuccess = true;
            }
            else
            {
                var result = await _sm23Repository.GetGroupPostCommentsAsync(
                    request.PostId,
                    request.UserId,
                    ct
                );
                response.GroupPostComments = result;
                response.StatusCode = SM23ResponseStatusCode.SUCCESS;
                response.IsSuccess = true;
            }
        }
        catch
        {
            response.StatusCode = SM23ResponseStatusCode.FAILED;
            response.UserPostComments = null;
            response.GroupPostComments = null;
            response.IsSuccess = false;
        }

        return response;
    }
}
