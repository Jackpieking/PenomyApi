using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG10;

public class G10Handler : IFeatureHandler<G10Request, G10Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G10Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G10Response> ExecuteAsync(G10Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G10Repository.GetCommentsAsync(
            long.Parse(request.ArtworkId),
            long.Parse(request.GetUserId()),
            int.Parse(request.CommentSection)
        );  

        return new G10Response { Result = result, StatusCode = G10ResponseStatusCode.SUCCESS };
    }
}
