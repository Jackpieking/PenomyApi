using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM27;

public class SM27Handler : IFeatureHandler<SM27Request, SM27Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public SM27Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<SM27Response> ExecuteAsync(SM27Request request, CancellationToken ct)
    {
        // var isPostAuthor = await _unitOfWork.Value.SM27Repository.CheckPostOnwerAsync(
        //     request.PostId,
        //     request.GetUserId(),
        //     ct
        // );

        // if (!isPostAuthor)
        //     return new SM27Response
        //     {
        //         IsSuccess = false,
        //         StatusCode = SM27ResponseStatusCode.DATABASE_ERROR,
        //     };

        var result = await _unitOfWork.Value.SM27Repository.TakeDownPostCommentsAsync(
            request.CommentId,
            ct
        );

        if (!result)
        {
            return new SM27Response
            {
                IsSuccess = false,
                StatusCode = SM27ResponseStatusCode.DATABASE_ERROR,
            };
        }
        return new SM27Response { IsSuccess = true, StatusCode = SM27ResponseStatusCode.SUCCESS };
    }
}
