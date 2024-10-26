using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG58;

public class G58Handler : IFeatureHandler<G58Request, G58Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G58Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<G58Response> ExecuteAsync(G58Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var idGenerator = _idGenerator.Value;
        request.ReplyComment.Id = idGenerator.Get();
        request.ReplyComment.UpdatedAt = DateTime.UtcNow;
        request.ReplyComment.TotalChildComments = 0;
        request.ReplyComment.TotalLikes = 0;
        request.ReplyComment.IsDirectlyCommented = false;
        var result = await unitOfWork.G58Repository.ExcecuteReplyCommentAsync(
            request.ReplyComment,
            request.ParentCommentId,
            new CancellationToken()
        );

        if (!result.Equals(string.Empty))
        {
            return new G58Response
            {
                CommentId = result,
                IsSuccess = true,
                StatusCode = G58ResponseStatusCode.SUCCESS,
            };
        }
        else if (result == 3)
        {
            return new G58Response
            {
                CommentId = result,
                IsSuccess = false,
                StatusCode = G58ResponseStatusCode.Comment_NotFound,
            };
        }
        else
        {
            return new G58Response
            {
                CommentId = result,
                IsSuccess = false,
                StatusCode = G58ResponseStatusCode.DATABASE_ERROR,
            };
        }
    }
}
