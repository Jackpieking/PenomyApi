using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SystemManagement;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.Sys1;

public class Sys1Handler : IFeatureHandler<Sys1Request, Sys1Response>
{
    private readonly ISys1Repository _Sys1Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Sys1Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _Sys1Repository = unitOfWork.Value.Sys1Repository;
        _idGenerator = idGenerator;
        _fileService = fileService;
    }

    public async Task<Sys1Response> ExecuteAsync(Sys1Request request, CancellationToken ct)
    {
        try
        {
            var generatedId = _idGenerator.Value.Get();

            var sysAccount = new SystemAccount { Id = generatedId };

            long createdId = await _Sys1Repository.CreateManagerAccountAsync(sysAccount, ct);

            if (createdId == 0)
                return new Sys1Response
                {
                    IsSuccess = false,
                    StatusCode = Sys1ResponseStatusCode.FAILED,
                };
            return new Sys1Response
            {
                IsSuccess = true,
                AccountId = createdId,
                StatusCode = Sys1ResponseStatusCode.SUCCESS,
            };
        }
        catch
        {
            return new Sys1Response
            {
                IsSuccess = false,
                StatusCode = Sys1ResponseStatusCode.FAILED,
            };
        }
    }
}
