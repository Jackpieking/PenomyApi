using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG56;

public class G56Handler : IFeatureHandler<G56Request, G56Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G56Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G56Response> ExecuteAsync(G56Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G56Repository.ExcecuteLikeCommentAsync(
            request.CommentId,
            request.UserId,
            new CancellationToken()
        );

        switch (result)
        {
            case 1:
            {
                return new G56Response
                {
                    IsSuccess = true,
                    StatusCode = G56ResponseStatusCode.SUCCESS,
                };
            }
            case 2:
            {
                return new G56Response
                {
                    IsSuccess = false,
                    StatusCode = G56ResponseStatusCode.DATABASE_ERROR,
                };
            }
            case 4:
            {
                return new G56Response
                {
                    IsSuccess = false,
                    StatusCode = G56ResponseStatusCode.NOT_FOUND,
                };
            }
            default:
            {
                return new G56Response
                {
                    IsSuccess = false,
                    StatusCode = G56ResponseStatusCode.INVALID_REQUEST,
                };
            }
        }
    }
}
