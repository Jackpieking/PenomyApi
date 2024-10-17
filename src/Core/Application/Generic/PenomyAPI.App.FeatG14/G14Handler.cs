using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG14;

public class G14Handler : IFeatureHandler<G14Request, G14Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G14Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G14Response> ExecuteAsync(G14Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G14Repository.GetComicsByCategoryAsync(request.Category);
        if (result == null)
            return new G14Response
            {
                Result = result,
                StatusCode = G14ResponseStatusCode.DATABASE_ERROR,
            };
        return new G14Response { Result = result, StatusCode = G14ResponseStatusCode.SUCCESS };
    }
}
