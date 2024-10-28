using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG59;

public class G59Handler : IFeatureHandler<G59Request, G59Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G59Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G59Response> ExecuteAsync(G59Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G59Repository.GetReplyCommentsAsync(
            request.ParentCommentId,
            request.UserId
        );

        return new G59Response { Result = result, StatusCode = G59ResponseStatusCode.SUCCESS };
    }
}
