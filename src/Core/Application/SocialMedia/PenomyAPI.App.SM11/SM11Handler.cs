using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM11;

public class SM11Handler : IFeatureHandler<SM11Request, SM11Response>
{
    private readonly ISM11Repository _sm11Repository;

    public SM11Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm11Repository = unitOfWork.Value.FeatSM11Repository;
    }

    public async Task<SM11Response> ExecuteAsync(SM11Request request, CancellationToken ct)
    {
        var response = new SM11Response();
        try
        {
            List<GroupPost> groupPosts;
            if (request.UserId != 0)
            {
                groupPosts = await _sm11Repository.GetGroupPostsAsync(
                    request.UserId,
                    request.GroupId,
                    ct
                );
                response.GroupPosts = groupPosts;
            }

            response.StatusCode = SM11ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.StatusCode = SM11ResponseStatusCode.FAILED;
        }

        return response;
    }
}
