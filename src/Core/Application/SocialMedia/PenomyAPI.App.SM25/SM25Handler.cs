using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM25;

public class SM25Handler : IFeatureHandler<SM25Request, SM25Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public SM25Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<SM25Response> ExecuteAsync(SM25Request request, CancellationToken ct)
    {
        var result = await _unitOfWork.Value.SM25Repository.UpdatePostCommentsAsync(
            request.CommentId,
            request.NewComment,
            request.GetUserId(),
            ct
        );

        if (result)
        {
            return new SM25Response
            {
                IsSuccess = true,
                StatusCode = SM25ResponseStatusCode.SUCCESS,
            };
        }
        else
        {
            return new SM25Response
            {
                IsSuccess = false,
                StatusCode = SM25ResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
