using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
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
        long result;
        if (request.IsGroupPostComment)
        {
            var groupPostComment = new GroupPostComment
            {
                Id = idGenerator.Get(),
                Content = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TotalChildComments = 0,
                IsDirectlyCommented = true,
                IsRemoved = false,
                PostId = request.PostId,
                CreatedBy = request.GetUserId(),
            };
            result = await unitOfWork.SM24Repository.CreateGroupPostCommentsAsync(
                groupPostComment,
                ct
            );
        }
        else
        {
            var userPostComment = new UserPostComment
            {
                Id = idGenerator.Get(),
                Content = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TotalChildComments = 0,
                TotalLikes = 0,
                IsDirectlyCommented = true,
                IsRemoved = false,
                PostId = request.PostId,
                CreatedBy = request.GetUserId(),
            };
            result = await unitOfWork.SM24Repository.CreateUserPostCommentsAsync(
                userPostComment,
                ct
            );
        }

        if (result != -1)
        {
            return new SM24Response
            {
                CommentId = result,
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
