using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM50;

public class SM50Handler : IFeatureHandler<SM50Request, SM50Response>
{
    private readonly IFeatChat1Repository _Chat1Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly ISM50Repository _sm50Repository;

    public SM50Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _sm50Repository = unitOfWork.Value.SM50Repository;
        _Chat1Repository = unitOfWork.Value.FeatChat1Repository;
        _idGenerator = idGenerator;
    }

    public async Task<SM50Response> ExecuteAsync(SM50Request request, CancellationToken ct)
    {
        var response = new SM50Response();
        try
        {
            response.IsSuccess = await _sm50Repository.LeaveSocialGroupAsync(
                request.UserId,
                request.GroupId,
                ct
            );
            response.StatusCode = SM50ResponseStatusCode.SUCCESS;
        }
        catch
        {
            response.IsSuccess = false;
            response.StatusCode = SM50ResponseStatusCode.FAILED;
        }
        return response;
    }
}
