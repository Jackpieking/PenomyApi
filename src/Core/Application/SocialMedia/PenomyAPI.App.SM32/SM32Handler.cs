using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM32;

public class SM32Handler : IFeatureHandler<SM32Request, SM32Response>
{
    private readonly ISM32Repository _sm32Repository;

    public SM32Handler(
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _sm32Repository = unitOfWork.Value.FeatSM32Repository;
    }

    public async Task<SM32Response> ExecuteAsync(SM32Request request, CancellationToken ct)
    {
        var response = new SM32Response();

        try
        {
            var userIds = await _sm32Repository.GetAllUserFriendsAsync(request.UserId, ct);
            var enumerable = userIds.ToList();
            if (enumerable.Count > 0)
                response.UserProfiles = await _sm32Repository.GetAllUserProfilesAsync(enumerable, ct);
            response.StatusCode = SM32ResponseStatusCode.SUCCESS;
        }
        catch
        {
            // Handle unexpected errors
            response.StatusCode = SM32ResponseStatusCode.FAILED;
        }

        return response;
    }
}
