using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM46;

public class SM46Handler : IFeatureHandler<SM46Request, SM46Response>
{
    private readonly ISM46Repository _sm46Repository;

    public SM46Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm46Repository = unitOfWork.Value.SM46Repository;
    }

    public async Task<SM46Response> ExecuteAsync(SM46Request request, CancellationToken ct)
    {
        // Check if user is group admin
        if (
            !await _sm46Repository.CheckUserRoleAsync(
                long.Parse(request.GroupId),
                long.Parse(request.GetUserId()),
                ct
            )
        )
            return new SM46Response
            {
                IsSuccess = false,
                StatusCode = SM46ResponseStatusCode.FORBIDDEN,
            };

        var response = new SM46Response();
        try
        {
            response.IsSuccess = await _sm46Repository.RejectJoinGroupRequestAsync(
                long.Parse(request.GroupId),
                long.Parse(request.MemberId),
                ct
            );

            response.StatusCode = SM46ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.IsSuccess = false;
            response.StatusCode = SM46ResponseStatusCode.FAILED;
        }

        return response;
    }
}
