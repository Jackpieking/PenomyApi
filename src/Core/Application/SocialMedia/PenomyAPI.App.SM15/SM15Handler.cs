using System;
using System.Collections.Generic;
using System.Linq;
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

    public SM15Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm15Repository = unitOfWork.Value.FeatSM15Repository;
    }

    public async Task<SM15Response> ExecuteAsync(SM15Request request, CancellationToken ct)
    {
        var response = new SM15Response();
        try
        {
            List<UserPost> userPosts;
            List<(bool, long)> isLikePostAsync;
            if (request.UserId != 0)
            {
                userPosts = await _sm15Repository.GetPersonalPostsAsync(request.UserId, ct);
                isLikePostAsync = await _sm15Repository.IsLikePostAsync(request.UserId,
                    userPosts.Select(x => x.Id.ToString()).ToList(), ct);
                response.UserPosts = userPosts;
                response.IsLikePostAsync = isLikePostAsync;
            }

            response.StatusCode = SM15ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.StatusCode = SM15ResponseStatusCode.FAILED;
        }

        return response;
    }
}
