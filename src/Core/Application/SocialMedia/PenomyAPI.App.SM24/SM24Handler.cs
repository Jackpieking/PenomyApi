using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM24;

public class SM24Handler : IFeatureHandler<SM24Request, SM24Response>
{
    private readonly ISM24Repository _sm24Repository;
    private readonly ISnowflakeIdGenerator _snowflakeIdGenerator;

    public SM24Handler(Lazy<IUnitOfWork> unitOfWork, ISnowflakeIdGenerator snowflakeIdGenerator)
    {
        _sm24Repository = unitOfWork.Value.SM24Repository;
        _snowflakeIdGenerator = snowflakeIdGenerator;
    }

    public async Task<SM24Response> ExecuteAsync(SM24Request request, CancellationToken ct)
    {
        var response = new SM24Response();

        request.comment.Id = _snowflakeIdGenerator.Get();
        request.comment.CreatedAt = DateTime.UtcNow;
        request.comment.CreatedBy = request.UserId;
        request.comment.TotalChildComments = 0;
        request.comment.TotalLikes = 0;
        request.comment.IsRemoved = false;
        request.comment.IsDirectlyCommented = true;
        
        try
        {
            var result = await _sm24Repository.CreatePostCommentsAsync(request.comment, ct);

            response.Comment = request.comment;
            response.StatusCode = SM24ResponseStatusCode.SUCCESS;
            response.IsSuccess = true;
        }
        catch
        {
            response.StatusCode = SM24ResponseStatusCode.FAILED;
            response.Comment = null;
            response.IsSuccess = false;
        }

        return response;
    }
}
