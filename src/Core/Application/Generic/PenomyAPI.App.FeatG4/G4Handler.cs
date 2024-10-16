using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG4;

public class G4Handler : IFeatureHandler<G4Request, G4Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G4Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G4Response> ExecuteAsync(G4Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;
        var result = await unitOfWork.G4Repository.GetComicsByCategoryAsync(request.Category);
        if(result == null) return new G4Response { Result = result, StatusCode = G4ResponseStatusCode.DATABASE_ERROR };
        return new G4Response { Result = result, StatusCode = G4ResponseStatusCode.SUCCESS };
    }
}
