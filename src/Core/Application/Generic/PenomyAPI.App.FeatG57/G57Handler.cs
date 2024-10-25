using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG57;

public class G57Handler : IFeatureHandler<G57Request, G57Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G57Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G57Response> ExecuteAsync(G57Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G57Repository.ExcecuteUnlikeCommentAsync(
            request.CommentId,
            request.UserId,
            new CancellationToken()
        );

        switch (result)
        {
            case 1:
            {
                return new G57Response
                {
                    IsSuccess = true,
                    StatusCode = G57ResponseStatusCode.SUCCESS,
                };
            }
            case 2:
            {
                return new G57Response
                {
                    IsSuccess = false,
                    StatusCode = G57ResponseStatusCode.DATABASE_ERROR,
                };
            }
            case 4:
            {
                return new G57Response
                {
                    IsSuccess = false,
                    StatusCode = G57ResponseStatusCode.NOT_FOUND,
                };
            }
            default:
            {
                return new G57Response
                {
                    IsSuccess = false,
                    StatusCode = G57ResponseStatusCode.INVALID_REQUEST,
                };
            }
        }
    }
}
