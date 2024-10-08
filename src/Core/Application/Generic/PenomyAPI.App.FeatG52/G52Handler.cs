using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG52;

public class G52Handler : IFeatureHandler<G52Request, G52Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G52Handler(Lazy<IUnitOfWork> unitOfWork, Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<G52Response> ExecuteAsync(G52Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var idGenerator = _idGenerator.Value;
        request.ArtworkComment.Id = idGenerator.Get();
        request.ArtworkComment.UpdatedAt = DateTime.UtcNow;
        var result = await unitOfWork.G52Repository.CreateCommentAsync(request.ArtworkComment);

        if (!result.Equals(string.Empty))
        {
            return new G52Response
            {
                CommentId = result,
                IsSuccess = true,
                StatusCode = G52ResponseStatusCode.SUCCESS
            };
        }
        else
        {
            return new G52Response
            {
                CommentId = result,
                IsSuccess = false,
                StatusCode = G52ResponseStatusCode.DATABASE_ERROR
            };
        }
    }
}
