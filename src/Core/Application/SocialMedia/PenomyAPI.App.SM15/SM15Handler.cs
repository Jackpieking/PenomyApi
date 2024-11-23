using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM15;

public class SM15Handler : IFeatureHandler<SM15Request, SM15Response>
{
    private readonly ISM15Repository _sm15Repository;

    public SM15Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _sm15Repository = unitOfWork.Value.FeatSM15Repository;
    }

    public async Task<SM15Response> ExecuteAsync(SM15Request request, CancellationToken ct)
    {
        var response = new SM15Response();
        try
        {
            List<UserPost> userPosts;
            if (request.UserId != 0)
            {
                userPosts = await _sm15Repository.GetPersonalPostsAsync(request.UserId, ct);
                response.UserPosts = userPosts;
            }
        }
        catch
        {
            response.StatusCode = SM15ResponseStatusCode.FAILED;
        }

        return response;
    }
}
