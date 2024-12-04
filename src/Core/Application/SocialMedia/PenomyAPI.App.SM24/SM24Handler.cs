using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM24;

public class SM24Handler : IFeatureHandler<SM24Request, SM24Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public SM24Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<SM24Response> ExecuteAsync(SM24Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var idGenerator = _idGenerator.Value;
        request.Comment.Id = idGenerator.Get();
        request.Comment.CreatedAt = DateTime.UtcNow;
        request.Comment.UpdatedAt = DateTime.UtcNow;
        request.Comment.TotalChildComments = 0;
        request.Comment.TotalLikes = 0;
        request.Comment.IsDirectlyCommented = true;
        request.Comment.IsRemoved = false;
        var result = await unitOfWork.SM24Repository.CreatePostCommentsAsync(request.Comment, ct);

        if (result != -1)
        {
            return new SM24Response
            {
                CommentId = request.Comment.Id,
                IsSuccess = true,
                StatusCode = SM24ResponseStatusCode.SUCCESS,
            };
        }
        else
        {
            return new SM24Response
            {
                CommentId = -1,
                IsSuccess = false,
                StatusCode = SM24ResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
