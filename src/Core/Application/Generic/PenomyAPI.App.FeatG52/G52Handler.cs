using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG52;

public class G52Handler : IFeatureHandler<G52Request, G52Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G52Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G52Response> ExecuteAsync(G52Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
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
