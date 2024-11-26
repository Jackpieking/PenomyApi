using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM41;

public class SM41Handler : IFeatureHandler<SM41Request, SM41Response>
{
    private readonly ISM41Repository _sm41Repository;

    public SM41Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm41Repository = unitOfWork.Value.SM41Repository;
    }

    public async Task<SM41Response> ExecuteAsync(SM41Request request, CancellationToken ct)
    {
        var response = await _sm41Repository.KickMemberAsync(request.GroupId, request.UserId);

        if (response == -1)
            return new SM41Response
            {
                IsSuccess = false,
                StatusCode = SM41ResponseStatusCode.FAILED,
            };

        return new SM41Response { IsSuccess = true, StatusCode = SM41ResponseStatusCode.SUCCESS };
    }
}
