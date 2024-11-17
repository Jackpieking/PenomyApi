using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM1;

public class SM1Handler : IFeatureHandler<SM1Request, SM1Response>
{
    private readonly ISM1Repository _SM1Repository;

    public SM1Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _SM1Repository = unitOfWork.Value.SM1Repository;
    }

    public async Task<SM1Response> ExecuteAsync(SM1Request request, CancellationToken ct)
    {
        try
        {
            UserProfile userProfile = await _SM1Repository
                .GetUserFrofileByUserIdAsync(
                    request.UserId,
                    ct
                    );

            return new SM1Response
            {
                IsSuccess = true,
                Result = userProfile,
                StatusCode = SM1ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new SM1Response
            {
                IsSuccess = false,
                StatusCode = SM1ResponseStatusCode.FAILED
            };
        }
    }
}
