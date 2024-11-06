using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG20;

public class G20Handler : IFeatureHandler<G20Request, G20Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G20Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G20Response> ExecuteAsync(G20Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G20Repository.GetCommentsAsync(request.ArtworkId);

        return new G20Response { Result = result, StatusCode = G20ResponseStatusCode.SUCCESS };
    }
}
