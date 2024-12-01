using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM9;

public class SM9Handler : IFeatureHandler<SM9Request, SM9Response>
{
    private readonly ISM9Repository _SM9Repository;

    public SM9Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM9Repository = unitOfWork.Value.SM9Repository;
    }

    public async Task<SM9Response> ExecuteAsync(SM9Request request, CancellationToken ct)
    {
        var groupList = await _SM9Repository.GetSocialGroupsAsync(
            long.Parse(request.UserId),
            request.MaxRecord,
            ct
        );
        return new SM9Response
        {
            StatusCode = SM9ResponseStatusCode.SUCCESS,
            Result = groupList,
            IsSuccess = true,
        };
    }
}
