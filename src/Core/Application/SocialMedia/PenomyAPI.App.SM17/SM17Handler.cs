using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM17;

public class SM17Handler : IFeatureHandler<SM17Request, SM17Response>
{
    private readonly ISM17Repository _sm17Repository;

    public SM17Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _sm17Repository = unitOfWork.Value.SM17Repository;
    }

    public async Task<SM17Response> ExecuteAsync(SM17Request request, CancellationToken ct)
    {
        var response = new SM17Response();
        try
        {
            var result = await _sm17Repository.LikeUnlikePostAsync(
                request.UserId,
                request.PostId,
                request.IsGroupPost,
                ct
            );

            if (result.StartsWith("1"))
            {
                response.IsLikeRequest = true;
                if (result.EndsWith("1"))
                {
                    response.IsSuccess = true;
                    response.StatusCode = SM17ResponseStatusCode.SUCCESS;
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = SM17ResponseStatusCode.FAILED;
                }
            }
            else if (result.StartsWith("0"))
            {
                response.IsLikeRequest = false;
                if (result.EndsWith("1"))
                {
                    response.IsSuccess = true;
                    response.StatusCode = SM17ResponseStatusCode.SUCCESS;
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = SM17ResponseStatusCode.FAILED;
                }
            }
        }
        catch
        {
            response.StatusCode = SM17ResponseStatusCode.FAILED;
        }

        return response;
    }
}
